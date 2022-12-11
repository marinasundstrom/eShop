namespace YourBrand.Orders.Domain.Events;

public sealed record CartCreated(string CartId) : DomainEvent;
