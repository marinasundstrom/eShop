using YourBrand.Catalog.Features.Attributes;
using YourBrand.Catalog.Features.Options;
using YourBrand.Catalog.Features.Products.Groups;

namespace YourBrand.Catalog.Features.Products;

public record class ProductDto(string Id, string Name, string? Description, ParentProductDto? Parent, ProductGroupDto? Group, string? Image, decimal Price, decimal? CompareAtPrice, int? QuantityAvailable, bool HasVariants, ProductVisibility? Visibility, IEnumerable<ProductAttributeDto> Attributes, IEnumerable<OptionDto> Options);

public record class ParentProductDto(string Id, string Name, string? Description, ProductGroupDto? Group);

public record class ProductAttributeDto(Attribute2Dto Attribute, AttributeValueDto? Value);