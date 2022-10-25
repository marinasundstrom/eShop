namespace YourBrand.Inventory.Domain.Entities;

public class Site : AuditableEntity
{
    protected Site() { }

    public Site(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}
