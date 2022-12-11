namespace YourBrand.Orders.Domain.Events;

public sealed record CartDeleted(string CartId) : DomainEvent;