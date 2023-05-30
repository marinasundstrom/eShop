namespace YourBrand.Payments.Domain.Events;

public sealed record CartUpdated(string CartId) : DomainEvent;