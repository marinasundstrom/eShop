using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Products.Variants;

public record GetProductVariantAttributes(string ProductId, string ProductVariantId) : IRequest<IEnumerable<ProductVariantAttributeDto>>
{
    public class Handler : IRequestHandler<GetProductVariantAttributes, IEnumerable<ProductVariantAttributeDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductVariantAttributeDto>> Handle(GetProductVariantAttributes request, CancellationToken cancellationToken)
        {
            var variantOptionValues = await _context.ProductAttributes
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Value)
                .Include(pv => pv.Attribute)
                //.ThenInclude(o => o.DefaultValue)
                .Include(pv => pv.Product)
                .ThenInclude(p => p.ParentProduct)
                .Where(pv => pv.Product.ParentProduct!.Id == request.ProductId && pv.Product.Id == request.ProductVariantId)
                .ToArrayAsync();

            return variantOptionValues.Select(x => x.ToDto());
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}