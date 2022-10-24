using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Options;

public record CreateItemOptionValue(string ItemId, string OptionId, ApiCreateItemOptionValue Data) : IRequest<OptionValueDto>
{
    public class Handler : IRequestHandler<CreateItemOptionValue, OptionValueDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionValueDto> Handle(CreateItemOptionValue request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .FirstAsync(x => x.Id == request.ItemId);

            var option = await _context.Options
                .FirstAsync(x => x.Id == request.OptionId);

            var value = new OptionValue
            {
                Name = request.Data.Name,
                SKU = request.Data.SKU,
                Price = request.Data.Price
            };

            option.Values.Add(value);

            await _context.SaveChangesAsync();

            return new OptionValueDto(value.Id, value.Name, value.SKU, value.Price, value.Seq);
        }
    }
}
