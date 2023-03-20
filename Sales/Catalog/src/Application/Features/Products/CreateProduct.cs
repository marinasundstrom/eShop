using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record CreateProduct(string Name, string Handle, string StoreId, bool HasVariants, string? Description,  long? GroupId, string? Sku, decimal? Price, ProductVisibility? Visibility) : IRequest<ProductDto?>
{
    public sealed class Handler : IRequestHandler<CreateProduct, ProductDto?>
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
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.Id == request.GroupId);

            var item = new Product(request.Name, request.Handle)
            {
                Name = request.Name,
                Description = request.Description,
                StoreId = request.StoreId,
                Group = group,
                Price = request.Price,
                HasVariants = request.HasVariants
            };

            group!.AddProductCount();

            /*
            foreach (var attribute in group!.Attributes)
            {
                item.ProductAttributes.Add(attribute);
            }
            */

            foreach (var option in group!.Options)
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