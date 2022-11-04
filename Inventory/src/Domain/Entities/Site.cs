namespace YourBrand.Inventory.Domain.Entities;

public class Site : Entity<string>, IAuditable
{
    protected Site() { }

    public Site(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Name { get; set; } = null!;

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
