namespace YourBrand.Portal.Domain.Events;

public sealed record TodoDescriptionUpdated(int TodoId, string? Description) : DomainEvent;