namespace YourBrand.Pricing.Domain.Events;

public sealed record CartDeleted(string CartId) : DomainEvent;