namespace YourBrand.Payments.Contracts;

public record UpdateStatus(string ReceiptId, ReceiptStatus Status);

public record ProductPriceChanged(string ProductId, decimal OldPrice, decimal NewPrice);
