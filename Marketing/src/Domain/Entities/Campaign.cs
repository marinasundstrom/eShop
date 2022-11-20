using YourBrand.Marketing.Domain.Events;

namespace YourBrand.Marketing.Domain.Entities;

public class Campaign : Entity<string>, IAuditable
{
    readonly HashSet<ProductOffer> _productOffers = new HashSet<ProductOffer>();

#nullable disable

    protected Campaign() : base() { }

#nullable restore

    public Campaign(string name)
    : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset StartDate { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public IReadOnlyCollection<ProductOffer> ProductOffers => _productOffers;

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
