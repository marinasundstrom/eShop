namespace YourBrand.Sales.Domain.Events;

public sealed record CartUpdated(string CartId) : DomainEvent;