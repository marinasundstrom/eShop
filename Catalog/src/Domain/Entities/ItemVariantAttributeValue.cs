namespace YourBrand.Catalog.Domain.Entities;

public class ItemAttributeValue : Entity<int>
{
    //public Item Item { get; set; } = null!;

    public Item Variant { get; set; } = null!;

    public Entities.Attribute Attribute { get; set; } = null!;

    public AttributeValue Value { get; set; } = null!;
}