using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Features.Options;

namespace YourBrand.Catalog.Features.Products.Options;

public record CreateProductOptionValue(string ProductId, string OptionId, ApiCreateProductOptionValue Data) : IRequest<OptionValueDto>
{
    public class Handler : IRequestHandler<CreateProductOptionValue, OptionValueDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionValueDto> Handle(CreateProductOptionValue request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
            .FirstAsync(x => x.Id == request.ProductId);

            var option = await _context.Options
                .FirstAsync(x => x.Id == request.OptionId);

            var value = new OptionValue(request.Data.Name)
            {
                SKU = request.Data.SKU,
                Price = request.Data.Price
            };

            (option as ChoiceOption)!.Values.Add(value);

            await _context.SaveChangesAsync();

            return new OptionValueDto(value.Id, value.Name, value.SKU, value.Price, value.Seq);
        }
    }
}