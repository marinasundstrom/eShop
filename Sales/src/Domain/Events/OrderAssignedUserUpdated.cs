namespace YourBrand.Sales.Domain.Events;

public sealed record OrderAssignedUserUpdated(int OrderNo, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;