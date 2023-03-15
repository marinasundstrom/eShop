using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Variants;

public record GetProductVariant(string ProductIdOrHandle, string ProductVariantIdOrHandle) : IRequest<ProductDto?>
{
    public class Handler : IRequestHandler<GetProductVariant, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(GetProductVariant request, CancellationToken cancellationToken)
        {
            bool isProductId = long.TryParse(request.ProductIdOrHandle, out var productId);
            bool isProductVariantId = long.TryParse(request.ProductVariantIdOrHandle, out var productVariantId);

            var query = _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Variants)
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Attribute)
                .ThenInclude(o => o.Values)
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Value)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => pv.Group)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => (pv as ChoiceOption)!.DefaultValue)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => (pv as ChoiceOption)!.Values)
                .AsQueryable();

            query = isProductId ? 
                query.Where(pv => pv.ParentProduct!.Id == productId)
                : query.Where(pv => pv.ParentProduct!.Handle == request.ProductIdOrHandle);

            var itemVariant = isProductVariantId ? 
                await query.FirstOrDefaultAsync(pv => pv!.Handle == request.ProductVariantIdOrHandle, cancellationToken)
                : await query.FirstOrDefaultAsync(pv => pv!.Id == productVariantId, cancellationToken);

            if (itemVariant is null) return null;

            return itemVariant.ToDto();
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}