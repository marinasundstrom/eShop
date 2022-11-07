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
         return new ItemDto(
                item.Id, 
                item.Name, 
                item.Description,
                item.ParentItem?.ToDto2(),
                item.Group?.ToDto(),
                GetImageUrl(item.Image), 
                item.Price.GetValueOrDefault(), 
                item.CompareAtPrice, 
                item.QuantityAvailable,
                item.HasVariants, 
                (ItemVisibility?)item.Visibility,
                item.Attributes.Select(x => x.ToDto()),
                item.Options.Select(x => x.ToDto()),
                item.AttributeValues.Select(x => x.ToDto()));
    }

    public static ParentItemDto ToDto2(this Domain.Entities.Item item)
    {
        return new ParentItemDto(
                item.Id, 
                item.Name, 
                item.Description,
                item.Group?.ToDto());
    }

    public static ItemGroupDto ToDto(this Domain.Entities.ItemGroup itemGroup)
    {
         return new ItemGroupDto(itemGroup.Id, itemGroup.Name, itemGroup.Description, itemGroup.Parent?.ToDto());
    }
 
    private static string? GetImageUrl(string? name)
    {
        return name is null ? null : $"/images/{name}";
    }
}
