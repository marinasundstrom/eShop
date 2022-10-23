namespace YourBrand.Orders.Domain.Events;

public sealed record OrderCreated(int OrderId) : DomainEvent;
