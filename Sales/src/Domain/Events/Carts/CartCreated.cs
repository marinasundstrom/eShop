namespace YourBrand.Sales.Domain.Events;

public sealed record CartCreated(string CartId) : DomainEvent;
