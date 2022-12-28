namespace YourBrand.Catalog.Domain.Entities;

public sealed class Brand : AggregateRoot<string>
{
    private Brand() { }

    public Brand(string id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; set; } = null!;
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