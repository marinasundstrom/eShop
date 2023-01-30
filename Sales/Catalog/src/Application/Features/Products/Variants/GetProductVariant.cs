using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Variants;

public record GetProductVariant(string ProductId, string ProductVariantId) : IRequest<ProductDto?>
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
            var itemVariant = await _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Variants)
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Attribute)
                //.ThenInclude(o => o.DefaultValue)
                .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Value)
                .FirstOrDefaultAsync(pv => pv.ParentProduct!.Id == request.ProductId && pv.Id == request.ProductVariantId);

            if (itemVariant is null) return null;

            return itemVariant.ToDto();
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}