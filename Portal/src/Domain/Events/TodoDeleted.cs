namespace YourBrand.Portal.Domain.Events;

public sealed record TodoDeleted(int TodoId, string Title) : DomainEvent;