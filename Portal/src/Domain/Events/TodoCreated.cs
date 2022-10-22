namespace YourBrand.Portal.Domain.Events;

public sealed record TodoCreated(int TodoId) : DomainEvent;
