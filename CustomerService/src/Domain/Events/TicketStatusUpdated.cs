using YourBrand.CustomerService.Domain.Enums;

namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketStatusUpdated(int TicketId, TicketStatus NewStatus, TicketStatus OldStatus) : DomainEvent;