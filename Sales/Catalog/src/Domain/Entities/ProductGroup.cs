namespace YourBrand.Catalog.Domain.Entities;

public class ProductGroup : Entity<long>
{
    protected ProductGroup() { }

    public ProductGroup(string name) : base(0)
    {
        Name = name;
    }

    public Store Store { get; set; } = null!;

    public string? StoreId { get; private set; }

    public int? Seq { get; set; }

    public string Name { get; set; } = null!;

    public string Handle { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string? Description { get; set; }

    public ProductGroup? Parent { get; set; }

    public string? Image { get; set; }

    public bool AllowItems { get; set; }

    public bool Hidden { get; set; }

    public int ProductsCount { get; private set; }

    public void IncrementProductCount()
    {
        ProductsCount++;

        Parent?.IncrementProductCount();
    }

    public void DecrementProductCount()
    {
        ProductsCount--;

        Parent?.DecrementProductCount();
    }

    public List<ProductGroup> SubGroups { get; } = new List<ProductGroup>();

    public List<Product> Products { get; } = new List<Product>();

    public List<Attribute> Attributes { get; } = new List<Attribute>();

    public List<Option> Options { get; } = new List<Option>();
}
