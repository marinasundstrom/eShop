using System.Collections.Generic;
using YourBrand.Sales.Domain.Events;

namespace YourBrand.Sales.Domain.Entities;

public class OrderStatus : AuditableEntity
{
    protected OrderStatus()
    {
    }

    public OrderStatus(string name)
    {
        Name = name;
    }

    public int Id { get; private set; }

    public string Name { get; set; } = null!;
}
