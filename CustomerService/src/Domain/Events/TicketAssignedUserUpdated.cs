namespace YourBrand.CustomerService.Domain.Events;

public sealed record TicketAssignedUserUpdated(int TicketId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;