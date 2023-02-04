using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application;

public static class Mapper
{
    public static SiteProductDto ToDto(this YourBrand.Catalog.ProductDto product, string? description = null) =>
        new SiteProductDto(product.Id, product.Name, !string.IsNullOrEmpty(description) ? description : product.Description, product.Parent?.ToDto2(), product.Group?.ToDto(), product.Image, product.Price, product.CompareAtPrice, product.QuantityAvailable, product.Attributes, product.Options, product.HasVariants);

    public static SiteParentProductDto ToDto2(this YourBrand.Catalog.ParentProductDto product) =>
        new SiteParentProductDto(product.Id, product.Name, product.Description, product.Group?.ToDto());

    public static SiteProductGroupDto ToDto(this YourBrand.Catalog.ProductGroupDto dto) => new SiteProductGroupDto(dto.Id, dto.Name, dto.Parent?.ToDto());
}