namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketUpdated(int TicketId) : DomainEvent;