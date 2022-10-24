namespace YourBrand.Sales.Domain.Events;

public sealed record OrderCreated(string OrderId) : DomainEvent;
