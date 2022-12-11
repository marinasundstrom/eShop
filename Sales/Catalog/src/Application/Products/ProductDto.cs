using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Products.Groups;

namespace YourBrand.Catalog.Application.Products;

public record class ProductDto(string Id, string Name, string? Description, ParentProductDto? Parent, ProductGroupDto? Group, string? Image, decimal Price, decimal? CompareAtPrice, int? QuantityAvailable, bool HasVariants, ProductVisibility? Visibility, IEnumerable<AttributeDto> Attributes2, IEnumerable<OptionDto> Options, IEnumerable<ProductVariantAttributeDto> Attributes);

public record class ParentProductDto(string Id, string Name, string? Description, ProductGroupDto? Group);