using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Products.Variants;

public class ProductsService
{
    private readonly IApplicationDbContext _context;

    public ProductsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> FindVariantCore(string productId, string? itemVariantId, IDictionary<string, string?> selectedAttributeValues)
    {
        var query = _context.Products
            .AsSplitQuery()
            .AsNoTracking()
            .Include(pv => pv.ParentProduct)
                .ThenInclude(pv => pv!.Group)
            .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Attribute)
            .Include(pv => pv.ProductAttributes)
                .ThenInclude(pv => pv.Value)
            .Where(pv => pv.ParentProduct!.Id == productId)
            .AsQueryable();

        if (itemVariantId is not null)
        {
            query = query.Where(pv => pv.Id != itemVariantId);
        }

        IEnumerable<Product> variants = await query
            .ToArrayAsync();

        foreach (var selectedOption in selectedAttributeValues)
        {
            if (selectedOption.Value is null)
                continue;

            variants = variants.Where(x => x.ProductAttributes.Any(vv => vv.Attribute.Id == selectedOption.Key && vv.Value?.Id == selectedOption.Value));
        }

        return variants;
    }
}