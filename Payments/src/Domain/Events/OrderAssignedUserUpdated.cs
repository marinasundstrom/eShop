namespace YourBrand.Payments.Domain.Events;

public sealed record ReceiptAssignedUserUpdated(string ReceiptId, string? AssignedUserId, string? OldAssignedUserId) : DomainEvent;