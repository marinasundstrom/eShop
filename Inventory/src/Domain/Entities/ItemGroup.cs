namespace YourBrand.Inventory.Domain.Entities;

public class ItemGroup : Entity<string>, IAuditable
{
    protected ItemGroup() { }

    public ItemGroup(string id)
    {
        Id = id;
    }

    public string Name { get; set; } = null!;

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}