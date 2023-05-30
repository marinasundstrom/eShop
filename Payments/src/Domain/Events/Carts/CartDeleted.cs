namespace YourBrand.Payments.Domain.Events;

public sealed record CartDeleted(string CartId) : DomainEvent;