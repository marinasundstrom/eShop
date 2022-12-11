namespace YourBrand.Pricing.Domain.Events;

public sealed record CartCreated(string CartId) : DomainEvent;
