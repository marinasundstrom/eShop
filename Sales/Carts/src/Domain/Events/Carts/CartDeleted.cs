namespace YourBrand.Carts.Domain.Events;

public sealed record CartDeleted(string CartId) : DomainEvent;