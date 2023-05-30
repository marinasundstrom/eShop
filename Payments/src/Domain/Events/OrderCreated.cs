namespace YourBrand.Payments.Domain.Events;

public sealed record ReceiptCreated(string ReceiptId) : DomainEvent;

public sealed record ProductPriceListCreated(string ProductPriceListId) : DomainEvent;