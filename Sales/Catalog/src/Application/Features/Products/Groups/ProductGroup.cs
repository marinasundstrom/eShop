namespace YourBrand.Catalog.Features.Products.Groups;

public record class ProductGroupDto(string Id, string Name, string? Description, ProductGroupDto? Parent);