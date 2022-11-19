namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketSubjectUpdated(int TicketId, string Title) : DomainEvent;