namespace YourBrand.Portal.Domain.Events;

public sealed record TodoTitleUpdated(int TodoId, string Title) : DomainEvent;