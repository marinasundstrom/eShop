namespace YourBrand.Orders.Domain.Events;

public sealed record CartUpdated(string CartId) : DomainEvent;