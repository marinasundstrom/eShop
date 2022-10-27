namespace YourBrand.Sales.Domain.Events;

public sealed record OrderDeleted(int OrderNo) : DomainEvent;