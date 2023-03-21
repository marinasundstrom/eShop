namespace YourBrand.Catalog.Domain.Entities;

public sealed class Brand : AggregateRoot<int>
{
    private Brand() : base(0) { }

    public Brand(string name, string handle) : base()
    {
        Name = name;
        Handle = handle;
    }

    public string Name { get; set; } = null!;

    public string Handle { get; set; } = null!;
}

/*
public sealed class Merchant : AggregateRoot<string>
{
    private Merchant() { }

    public Merchant(string id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; set; } = null!;
}
*/