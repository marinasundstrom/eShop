using System.Collections.Generic;
using YourBrand.Orders.Domain.Events;

namespace YourBrand.Orders.Domain.Entities;

public class OrderStatus : Entity<int>, IAuditable
{
    protected OrderStatus()
    {
    }

    public OrderStatus(int id)
    {
        Id = id;
    }

    public OrderStatus(string name)
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
