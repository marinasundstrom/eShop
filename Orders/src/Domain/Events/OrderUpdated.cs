namespace YourBrand.Orders.Domain.Events;

public sealed record OrderUpdated(int OrderId) : DomainEvent;