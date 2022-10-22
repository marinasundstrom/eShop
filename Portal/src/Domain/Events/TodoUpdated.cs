namespace YourBrand.Portal.Domain.Events;

public sealed record TodoUpdated(int TodoId) : DomainEvent;