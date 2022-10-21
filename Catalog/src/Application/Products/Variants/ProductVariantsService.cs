using Microsoft.EntityFrameworkCore;

using Catalog.Domain;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Variants;

public class ProductVariantsService 
{
    private readonly IApplicationDbContext _context;

    public ProductVariantsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductVariant>> FindVariantCore(string productId, string? productVariantId, IDictionary<string, string?> selectedAttributeValues)
    {
        var query = _context.ProductVariants
            .AsSplitQuery()
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Include(pv => pv.AttributeValues)
            .ThenInclude(pv => pv.Attribute)
            .Include(pv => pv.AttributeValues)
            .ThenInclude(pv => pv.Value)
            .Where(pv => pv.Product.Id == productId)
            .AsQueryable();

        if (productVariantId is not null)
        {
            query = query.Where(pv => pv.Id != productVariantId);
        }

        IEnumerable<ProductVariant> variants = await query
            .ToArrayAsync();

        foreach (var selectedOption in selectedAttributeValues)
        {
            if (selectedOption.Value is null)
                continue;

            variants = variants.Where(x => x.AttributeValues.Any(vv => vv.Attribute.Id == selectedOption.Key && vv.Value.Id == selectedOption.Value));
        }

        return variants;
    }
}