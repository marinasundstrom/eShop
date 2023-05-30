namespace YourBrand.Payments.Domain.Events;

public sealed record ReceiptDeleted(int ReceiptNo) : DomainEvent;