namespace YourBrand.Catalog.Domain.Entities;

public class ItemGroup : Entity<string>
{
    protected ItemGroup() {}

    public ItemGroup(string id, string name) : base(id)
    {
        Name = name;
    }

    public Store Store { get; set; } = null!;

    public string? StoreId { get; private set; }

    public int? Seq { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ItemGroup? Parent { get; set; }

    public string? Image { get; set; }

    public bool Hidden { get; set; }

    public List<ItemGroup> SubGroups { get; } = new List<ItemGroup>();

    public List<Item> Items { get; } = new List<Item>();

    public List<Item> Items2 { get; } = new List<Item>();

    public List<Item> Items3 { get; } = new List<Item>();

    public List<Attribute> Attributes { get; } = new List<Attribute>();

    public List<Option> Options { get; } = new List<Option>();
}
