namespace YourBrand.Sales.Domain.Events;

public sealed record OrderCreated(int OrderNo) : DomainEvent;
