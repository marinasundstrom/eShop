namespace YourBrand.Subscriptions.Domain.Events;

public sealed record CartDeleted(string CartId) : DomainEvent;