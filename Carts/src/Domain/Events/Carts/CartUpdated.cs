namespace YourBrand.Carts.Domain.Events;

public sealed record CartUpdated(string CartId) : DomainEvent;