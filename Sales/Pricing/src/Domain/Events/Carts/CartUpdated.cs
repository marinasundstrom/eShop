namespace YourBrand.Pricing.Domain.Events;

public sealed record CartUpdated(string CartId) : DomainEvent;