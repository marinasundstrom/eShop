using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Variants;

public class ItemsService 
{
    private readonly IApplicationDbContext _context;

    public ItemsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Item>> FindVariantCore(string itemId, string? itemVariantId, IDictionary<string, string?> selectedAttributeValues)
    {
        var query = _context.Items
            .AsSplitQuery()
            .AsNoTracking()
            .Include(pv => pv.ParentItem)
            .Include(pv => pv.AttributeValues)
            .ThenInclude(pv => pv.Attribute)
            .Include(pv => pv.AttributeValues)
            .ThenInclude(pv => pv.Value)
            .Where(pv => pv.ParentItem!.Id == itemId)
            .AsQueryable();

        if (itemVariantId is not null)
        {
            query = query.Where(pv => pv.Id != itemVariantId);
        }

        IEnumerable<Item> variants = await query
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