namespace YourBrand.Payments.Domain.Events;

public sealed record ReceiptUpdated(string ReceiptId) : DomainEvent;
