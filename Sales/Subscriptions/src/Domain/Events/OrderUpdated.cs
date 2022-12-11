namespace YourBrand.Subscriptions.Domain.Events;

public sealed record OrderUpdated(string OrderId) : DomainEvent;
