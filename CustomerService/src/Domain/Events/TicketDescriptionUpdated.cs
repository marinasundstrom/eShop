namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketDescriptionUpdated(int TicketId, string? Description) : DomainEvent;