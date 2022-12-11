namespace YourBrand.Subscriptions.Domain.Events;

public sealed record CartCreated(string CartId) : DomainEvent;
