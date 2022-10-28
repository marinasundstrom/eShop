using YourBrand.Catalog.Application.Items;
using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Variants;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application;

public static class Mapping
{
    public static ItemDto ToDto(this Domain.Entities.Item item)
    {
         return new ItemDto(item.Id, item.Name, item.Description,
                item.Group is not null ? new ItemGroupDto(item.Group.Id, item.Group.Name, item.Group.Description, item.Group?.Parent?.Id) : null,
                GetImageUrl(item.Image), 
                item.Price.GetValueOrDefault(), 
                item.CompareAtPrice, 
                item.HasVariants, 
                (ItemVisibility?)item.Visibility,
                item.Attributes.Select(x => x.ToDto()),
                item.Options.Select(x => x.ToDto()),
                item.AttributeValues.Select(x => x.ToDto()));
    }
 
    private static string? GetImageUrl(string? name)
    {
        return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
    }
}
