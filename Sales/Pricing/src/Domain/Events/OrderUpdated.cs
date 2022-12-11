namespace YourBrand.Pricing.Domain.Events;

public sealed record OrderUpdated(string OrderId) : DomainEvent;
