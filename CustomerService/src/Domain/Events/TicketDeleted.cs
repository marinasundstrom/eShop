namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketDeleted(int TicketId, string Title) : DomainEvent;