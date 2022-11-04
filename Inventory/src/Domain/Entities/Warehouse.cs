namespace YourBrand.Inventory.Domain.Entities;

public class Warehouse : Entity<string>, IAuditable
{
    protected Warehouse() { }

    public Warehouse(string id, string name, string siteId)
    {
        Id = id;
        Name = name;
        SiteId = siteId;
    }

    public string Name { get; set; } = null!;

    public string SiteId { get; set; } = null!;

    public Site Site { get; set; } = null!;

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
