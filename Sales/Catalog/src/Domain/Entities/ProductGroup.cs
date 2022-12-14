namespace YourBrand.Catalog.Domain.Entities;

public class ProductGroup : Entity<string>
{
    protected ProductGroup() { }

    public ProductGroup(string id, string name) : base(id)
    {
        Name = name;
    }

    public Store Store { get; set; } = null!;

    public string? StoreId { get; private set; }

    public int? Seq { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ProductGroup? Parent { get; set; }

    public string? Image { get; set; }

    public bool Hidden { get; set; }

    public List<ProductGroup> SubGroups { get; } = new List<ProductGroup>();

    public List<Product> Products { get; } = new List<Product>();

    public List<Product> Products2 { get; } = new List<Product>();

    public List<Product> Products3 { get; } = new List<Product>();

    public List<Attribute> Attributes { get; } = new List<Attribute>();

    public List<Option> Options { get; } = new List<Option>();
}
