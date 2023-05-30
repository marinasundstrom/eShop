using System.Collections.Generic;
using YourBrand.Payments.Domain.Events;

namespace YourBrand.Payments.Domain.Entities;

public class ReceiptStatus : Entity<int>, IAuditable
{
    protected ReceiptStatus()
    {
    }

    public ReceiptStatus(int id)
    {
        Id = id;
    }

    public ReceiptStatus(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = null!;

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
