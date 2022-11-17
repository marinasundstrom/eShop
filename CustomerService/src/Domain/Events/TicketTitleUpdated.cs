namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketTitleUpdated(int TicketId, string Title) : DomainEvent;