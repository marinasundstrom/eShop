namespace YourBrand.Catalog.Domain.Entities;

public class ProductAttribute : Entity<int>
{
    public string ProductId { get; set; } = null!;

    public Product Product { get; set; } = null!;

    public string AttributeId { get; set; } = null!;

    public Entities.Attribute Attribute { get; set; } = null!;

    public List<ProductAttributeValue> Values { get; set; } = new List<ProductAttributeValue>();
}
