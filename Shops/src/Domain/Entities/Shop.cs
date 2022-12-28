using System.Collections.Generic;
using YourBrand.Shops.Domain.Events;

namespace YourBrand.Shops.Domain.Entities;

public class Shop : AggregateRoot<string>, IAuditable
{
    public Shop() : base(Guid.NewGuid().ToString())
    {
        
    }

    public string Name { get; private set; } = default!;

    public bool VatIncluded { get; set; }

    public string Currency { get; set; } = "SEK";

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
