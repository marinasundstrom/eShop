namespace YourBrand.Catalog.Domain.Entities;

public class OptionValue : Entity<string>
{
    protected OptionValue() {}

    public OptionValue(string name) 
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public int? Seq { get; set; }

    public Option Option { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ItemId { get; set; }

    public decimal? Price { get; set; }
}
