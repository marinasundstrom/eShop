namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketRemainingHoursUpdated(int TicketId, double? hHurs, double? OldHours) : DomainEvent;