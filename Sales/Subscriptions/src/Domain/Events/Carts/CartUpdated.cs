namespace YourBrand.Subscriptions.Domain.Events;

public sealed record CartUpdated(string CartId) : DomainEvent;