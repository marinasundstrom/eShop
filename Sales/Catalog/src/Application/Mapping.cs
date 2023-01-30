using YourBrand.Catalog.Features.Attributes;
using YourBrand.Catalog.Features.Options;
using YourBrand.Catalog.Features.Products;
using YourBrand.Catalog.Features.Products.Groups;
using YourBrand.Catalog.Features.Products.Variants;
using YourBrand.Catalog.Features.Stores;

namespace YourBrand.Catalog;

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
               item.ProductAttributes.Select(x => x.ToDto()),
               item.Options.Select(x => x.ToDto()));
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

    public static ProductAttributeDto ToDto(this Domain.Entities.ProductAttribute x)
    {
        return new ProductAttributeDto(x.Attribute.ToDto2(), x.Value?.ToDto());
    }
}