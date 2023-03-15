namespace YourBrand.Catalog.Features.Products.Groups;

public record class ProductGroupTreeNodeDto(long Id, string Name, string Handle, string Path, string? Description, ParentProductGroupDto? Parent, IEnumerable<ProductGroupTreeNodeDto> SubGroups);

public record class ProductGroupDto(long Id, string Name, string Handle, string Path, string? Description, ParentProductGroupDto? Parent);

public record class ParentProductGroupDto(long Id, string Name, string Handle, string Path, ParentProductGroupDto? Parent);