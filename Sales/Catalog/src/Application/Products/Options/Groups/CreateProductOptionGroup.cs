using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Products.Options.Groups;

public record CreateProductOptionGroup(string ProductId, ApiCreateProductOptionGroup Data) : IRequest<OptionGroupDto>
{
    public class Handler : IRequestHandler<CreateProductOptionGroup, OptionGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionGroupDto> Handle(CreateProductOptionGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            var group = new OptionGroup(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                Min = request.Data.Min,
                Max = request.Data.Max
            };

            item.OptionGroups.Add(group);

            await _context.SaveChangesAsync();

            return new OptionGroupDto(group.Id, group.Name, group.Description, group.Seq, group.Min, group.Max);
        }
    }
}
