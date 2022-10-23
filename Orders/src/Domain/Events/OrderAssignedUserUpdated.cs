namespace YourBrand.Orders.Domain.Events;

public sealed record OrderAssignedUserUpdated(int OrderId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;