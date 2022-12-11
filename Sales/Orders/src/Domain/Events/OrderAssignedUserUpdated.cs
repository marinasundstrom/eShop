namespace YourBrand.Orders.Domain.Events;

public sealed record OrderAssignedUserUpdated(string OrderId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;