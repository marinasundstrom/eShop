namespace YourBrand.Pricing.Domain.Events;

public sealed record OrderDeleted(int OrderNo) : DomainEvent;