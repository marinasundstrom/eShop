namespace YourBrand.Catalog.Domain.Entities;

public class Store : Entity<string>
{
    protected Store() { }

    public Store(string name, string handle, string currency) : base(Guid.NewGuid().ToString())
    {
        Name = name;
        Handle = handle;
        //Currency = currency;
    }

    public string Name { get; set; } = null!;

    public string Handle { get; set; } = null!;

    public Currency Currency { get; set; } = null!;

    public List<Product> Products { get; } = new List<Product>();
}
