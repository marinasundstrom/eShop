using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Products.Groups;
using YourBrand.Catalog.Application.Products.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application.Products;

public record CreateProduct(string? Id, string Name, bool HasVariants, string? Description, string? GroupId, decimal? Price, ProductVisibility? Visibility) : IRequest<ProductDto?>
{
    public class Handler : IRequestHandler<CreateProduct, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            var group = await _context.ProductGroups
                .Include(x => x.Attributes)
                .Include(x => x.Options)
                .FirstOrDefaultAsync(x => x.Id == request.GroupId);

            var item = new Product(request.Id ?? Guid.NewGuid().ToString(), request.Name)
            {
                Name = request.Name,
                Description = request.Description,
                Group = group,
                Price = request.Price,
                HasVariants = request.HasVariants
            };

            foreach (var attribute in group!.Attributes)
            {
                item.Attributes.Add(attribute);
            }

            foreach (var option in group.Options)
            {
                item.Options.Add(option);
            }

            if (request.Visibility == null)
            {
                item.Visibility = Domain.Enums.ProductVisibility.Unlisted;
            }
            else
            {
                item.Visibility = request.Visibility == ProductVisibility.Listed ? Domain.Enums.ProductVisibility.Listed : Domain.Enums.ProductVisibility.Unlisted;
            }

            _context.Products.Add(item);

            await _context.SaveChangesAsync();

            return item.ToDto();
        }
    }
}
