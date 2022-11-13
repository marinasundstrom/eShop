namespace YourBrand.Carts.Domain.Events;

public sealed record CartCreated(string CartId) : DomainEvent;
