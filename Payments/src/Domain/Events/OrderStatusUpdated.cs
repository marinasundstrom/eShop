using YourBrand.Payments.Domain.Entities;

namespace YourBrand.Payments.Domain.Events;

public sealed record ReceiptStatusUpdated(string ReceiptId, ReceiptStatus NewStatus, ReceiptStatus OldStatus) : DomainEvent;