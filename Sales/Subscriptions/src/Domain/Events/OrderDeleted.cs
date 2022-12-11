namespace YourBrand.Subscriptions.Domain.Events;

public sealed record OrderDeleted(int OrderNo) : DomainEvent;