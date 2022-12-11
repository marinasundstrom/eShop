namespace YourBrand.Catalog.Domain.Entities;

public class AttributeValue : Entity<string>
{
    protected AttributeValue() { }

    public AttributeValue(string name)
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public int? Seq { get; set; }

    public Attribute Attribute { get; set; } = null!;

    public string Name { get; set; } = null!;

    public List<ProductAttributeValue> ProductAttributeValues { get; } = new List<ProductAttributeValue>();
}
