namespace YourBrand.Sales.Domain.Events;

public sealed record OrderDeleted(string OrderId) : DomainEvent;