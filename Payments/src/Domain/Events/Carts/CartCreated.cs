namespace YourBrand.Payments.Domain.Events;

public sealed record CartCreated(string CartId) : DomainEvent;
