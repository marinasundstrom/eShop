using YourBrand.Catalog.Application.Products;
using YourBrand.Catalog.Application.Products.Groups;
using YourBrand.Catalog.Application.Products.Variants;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Stores;

namespace YourBrand.Catalog.Application;

public static class Mapping
{
    public static StoreDto ToDto(this Domain.Entities.Store store)
    {
        return new StoreDto(store.Id, store.Name, store.Handle);
    }

    public static ProductDto ToDto(this Domain.Entities.Product item)
    {
        return new ProductDto(
               item.Id,
               item.Name,
               item.Description,
               item.ParentProduct?.ToDto2(),
               item.Group?.ToDto(),
               GetImageUrl(item.Image),
               item.Price.GetValueOrDefault(),
               item.CompareAtPrice,
               item.QuantityAvailable,
               item.HasVariants,
               (ProductVisibility?)item.Visibility,
               item.Attributes.Select(x => x.ToDto()),
               item.Options.Select(x => x.ToDto()),
               item.AttributeValues.Select(x => x.ToDto()));
    }

    public static ParentProductDto ToDto2(this Domain.Entities.Product item)
    {
        return new ParentProductDto(
                item.Id,
                item.Name,
                item.Description,
                item.Group?.ToDto());
    }

    public static ProductGroupDto ToDto(this Domain.Entities.ProductGroup itemGroup)
    {
        return new ProductGroupDto(itemGroup.Id, itemGroup.Name, itemGroup.Description, itemGroup.Parent?.ToDto());
    }

    private static string? GetImageUrl(string? name)
    {
        return name is null ? null : $"/images/{name}";
    }
}
