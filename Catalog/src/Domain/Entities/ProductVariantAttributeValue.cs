namespace YourBrand.Catalog.Domain.Entities;

public class ProductAttributeValue : Entity<int>
{
    //public Product Product { get; set; } = null!;

    public Product Variant { get; set; } = null!;

    public Entities.Attribute Attribute { get; set; } = null!;

    public AttributeValue Value { get; set; } = null!;
}