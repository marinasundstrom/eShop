using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Products.Groups;

namespace YourBrand.Catalog.Application.Products;

public record class ProductDto(string Id, string Name, string? Description, ParentProductDto? Parent, ProductGroupDto? Group, string? Image, decimal Price, decimal? CompareAtPrice, int? QuantityAvailable, bool HasVariants, ProductVisibility? Visibility, IEnumerable<ProductAttributeDto> Attributes, IEnumerable<OptionDto> Options);

public record class ParentProductDto(string Id, string Name, string? Description, ProductGroupDto? Group);

public record class ProductAttributeDto(Attribute2Dto Attribute, AttributeValueDto? Value);