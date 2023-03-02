using YourBrand.Catalog.Features.Attributes;
using YourBrand.Catalog.Features.Options;
using YourBrand.Catalog.Features.Products.Groups;

namespace YourBrand.Catalog.Features.Products;

public record class ProductDto(long Id, string Name, string Handle, string? Description, ParentProductDto? Parent, ProductGroupDto? Group, string? Image, decimal Price, decimal? CompareAtPrice, int? QuantityAvailable, bool HasVariants, ProductVisibility? Visibility, IEnumerable<ProductAttributeDto> Attributes, IEnumerable<ProductOptionDto> Options);

public record class ParentProductDto(long Id, string Name, string Handle, string? Description, ProductGroupDto? Group);

public record class ProductAttributeDto(AttributeDto Attribute, AttributeValueDto? Value, bool ForVariant, bool IsMainAttribute);

public record class ProductOptionDto(OptionDto Option, bool IsInherited);