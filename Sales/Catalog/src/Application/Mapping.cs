using YourBrand.Catalog.Features.Attributes;
using YourBrand.Catalog.Features.Brands;
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
        return new StoreDto(store.Id, store.Name, store.Handle, store.Currency.Code);
    }

    public static ProductDto ToDto(this Domain.Entities.Product item)
    {
        return new ProductDto(
               item.Id,
               item.Name,
               item.Handle,
               item.Headline,
               item.SKU,
               item.ShortDescription,
               item.Description,
               item.Brand?.ToDto(),
               item.ParentProduct?.ToDto2(),
               item.Group?.ToDto(),
               GetImageUrl(item.Image),
               item.Price,
               item.RegularPrice,
               item.QuantityAvailable,
               item.IsCustomizable.GetValueOrDefault(),
               item.HasVariants,
               (ProductVisibility?)item.Visibility,
               item.ProductAttributes.Select(x => x.ToDto()),
               item.ProductOptions.Select(x => x.ToDto()));
    }

    public static ParentProductDto ToDto2(this Domain.Entities.Product item)
    {
        return new ParentProductDto(
                item.Id,
                item.Name,
                item.Handle,
                item.SKU,
                item.Description,
                item.Brand?.ToDto(),
                item.Group?.ToDto());
    }

    public static ProductGroupDto ToDto(this Domain.Entities.ProductGroup itemGroup)
    {
        return new ProductGroupDto(itemGroup.Id, itemGroup.Name, itemGroup.Handle, itemGroup.Path, itemGroup.Description, itemGroup.Parent?.ToDto2(), itemGroup.AllowItems);
    }

    public static ProductGroupTreeNodeDto ToDto3(this Domain.Entities.ProductGroup itemGroup)
    {
        return new ProductGroupTreeNodeDto(itemGroup.Id, itemGroup.Name, itemGroup.Handle, itemGroup.Path, itemGroup.Description, itemGroup.Parent?.ToDto2(), itemGroup.SubGroups.Select(x => x.ToDto3()), itemGroup.ProductsCount, itemGroup.AllowItems);
    }

    public static ParentProductGroupDto ToDto2(this Domain.Entities.ProductGroup itemGroup)
    {
        return new ParentProductGroupDto(itemGroup.Id, itemGroup.Name, itemGroup.Handle, itemGroup.Path, itemGroup.Parent?.ToDto2());
    }

    private static string? GetImageUrl(string? name)
    {
        return name is null ? null : $"/images/{name}";
    }

    public static ProductAttributeDto ToDto(this Domain.Entities.ProductAttribute x)
    {
        return new ProductAttributeDto(x.Attribute.ToDto(), x.Value?.ToDto(), x.ForVariant, x.IsMainAttribute);
    }

    public static ProductOptionDto ToDto(this Domain.Entities.ProductOption x)
    {
        return new ProductOptionDto(x.Option.ToDto(), x.IsInherited.GetValueOrDefault());
    }

    public static BrandDto ToDto(this Domain.Entities.Brand brand)
    {
        return new BrandDto(brand.Id, brand.Name, brand.Handle);
    }
}