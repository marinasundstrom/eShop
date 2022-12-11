namespace YourBrand.Orders.Domain.Events;

public sealed record OrderUpdated(string OrderId) : DomainEvent;
