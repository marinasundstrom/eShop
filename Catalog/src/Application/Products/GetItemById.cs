using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Products.Groups;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Products;

public record GetItemByItemId(string ItemId) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<GetItemByItemId, ItemDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemDto?> Handle(GetItemByItemId request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Group)
                .FirstOrDefaultAsync(p => p.ItemId == request.ItemId);

            if (product == null) 
            {
                var productVariant = await _context.ProductVariants
                    .AsSplitQuery()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ItemId == request.ItemId);

                if (productVariant == null) return null;

                return new ItemDto(productVariant.ItemId!, productVariant.Name, productVariant.Description, null,
                    GetImageUrl(productVariant.Image), productVariant.Price);   
            }

            return new ItemDto(product.ItemId!, product.Name, product.Description, product.Group == null ? null : new ProductGroupDto(product.Group.Id, product.Group.Name, product.Group.Description, product.Group?.Parent?.Id),
                GetImageUrl(product.Image), product.Price);
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
