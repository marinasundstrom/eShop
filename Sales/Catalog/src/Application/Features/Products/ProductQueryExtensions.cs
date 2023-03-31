using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public static class ProductQueryExtensions
{
    public static IQueryable<Product> IncludeAll(this IQueryable<Product> source)
    {
        return source.Include(pv => pv.ParentProduct)
                    .ThenInclude(pv => pv!.Group)
                    .ThenInclude(pv => pv!.Parent)
                .Include(pv => pv.Brand)
                .Include(pv => pv.Group)
                    .ThenInclude(pv => pv!.Parent)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Value)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Attribute)
                    .ThenInclude(pv => pv.Values)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Attribute)
                    .ThenInclude(o => o.Group)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Attribute)
                    .ThenInclude(pv => pv.Values)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Value)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => pv.Group)
                .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => (pv as ChoiceOption)!.Values)
                .Include(pv => pv.Options)
                    .ThenInclude(pv => (pv as ChoiceOption)!.DefaultValue);
    }
}