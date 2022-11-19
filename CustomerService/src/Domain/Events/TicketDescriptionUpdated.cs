namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketTextUpdated(int TicketId, string? Description) : DomainEvent;