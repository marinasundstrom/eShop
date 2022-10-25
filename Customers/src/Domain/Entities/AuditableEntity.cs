namespace YourBrand.Customers.Domain.Entities;

public abstract class AuditableEntity : Entity
{
    public string? CreatedById { get; set; }
    public DateTimeOffset? Created { get; set; }

    public string? LastModifiedById { get; set; }
    public DateTimeOffset? LastModified { get; set; }
}