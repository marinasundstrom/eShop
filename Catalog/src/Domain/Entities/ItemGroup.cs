namespace YourBrand.Catalog.Domain.Entities;

public class ItemGroup : IAggregateRoot
{
    public string Id { get; set; } = null!;

    public int? Seq { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ItemGroup? Parent { get; set; }

    public string? Image { get; set; }

    public List<ItemGroup> SubGroups { get; } = new List<ItemGroup>();

    public List<Item> Items { get; } = new List<Item>();
}
