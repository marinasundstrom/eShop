namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketCreated(int TicketId) : DomainEvent;
