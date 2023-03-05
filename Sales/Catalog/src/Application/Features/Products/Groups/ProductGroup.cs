namespace YourBrand.Catalog.Features.Products.Groups;

public record class ProductGroupDto(long Id, string Name, string Handle, string? Description, ProductGroupDto? Parent);