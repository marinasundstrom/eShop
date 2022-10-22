namespace YourBrand.Portal.Domain.Events;

public sealed record TodoRemainingHoursUpdated(int TodoId, double? hHurs, double? OldHours) : DomainEvent;