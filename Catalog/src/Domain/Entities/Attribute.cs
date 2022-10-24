namespace YourBrand.Catalog.Domain.Entities;

public class Attribute : IAggregateRoot
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public AttributeGroup? Group { get; set; }

    public bool ForVariant { get; set; }

    public bool IsMainAttribute { get; set; }

    public List<Item> Items { get; } = new List<Item>();

    public List<AttributeValue> Values { get; } = new List<AttributeValue>();
}
