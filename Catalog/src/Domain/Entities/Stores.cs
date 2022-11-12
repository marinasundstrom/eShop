namespace YourBrand.Catalog.Domain.Entities;

public class Store : Entity<string>
{
    protected Store() {}

    public Store(string name, string handle) : base(Guid.NewGuid().ToString())
    { 
        Name = name;
        Handle = handle;
    }
    
    public string Name { get; set; } = null!;

    public string Handle { get; private set; } = null!;


    public List<Item> Items { get; } = new List<Item>();
}