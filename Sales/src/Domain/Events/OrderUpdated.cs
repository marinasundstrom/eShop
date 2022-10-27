namespace YourBrand.Sales.Domain.Events;

public sealed record OrderUpdated(int OrderNo) : DomainEvent;