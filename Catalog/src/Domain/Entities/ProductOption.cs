namespace YourBrand.Catalog.Domain.Entities;

public class ProductOption : Entity<int>
{
    public string ProductId { get; set; } = null!;

    public Product Product { get; set; } = null!;

    public string OptionId { get; set; } = null!;

    public Option Option { get; set; } = null!;

    public bool? IsSelected { get; set; }

    // Add fields for default values
}