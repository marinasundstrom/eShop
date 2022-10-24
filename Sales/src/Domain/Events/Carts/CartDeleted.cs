namespace YourBrand.Sales.Domain.Events;

public sealed record CartDeleted(string CartId) : DomainEvent;