namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketEstimatedHoursUpdated(int TicketId, double? Hours, double? OldHours) : DomainEvent;