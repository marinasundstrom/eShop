using YourBrand.CustomerService.Domain.Entities;

namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketStatusUpdated(int TicketId, TicketStatus NewStatus, TicketStatus OldStatus) : DomainEvent;