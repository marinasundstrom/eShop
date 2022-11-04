namespace YourBrand.Catalog.Domain.Entities;

public class ItemAttribute : Entity<int>
{
    public string ItemId { get; set; } = null!;

    public Item Item { get; set; } = null!;

    public string AttributeId { get; set; } = null!;

    public Entities.Attribute Attribute { get; set; } = null!;

}
