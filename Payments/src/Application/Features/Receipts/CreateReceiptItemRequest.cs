namespace YourBrand.Payments.Application.Features.Receipts;

public sealed record CreateReceiptItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, string? Notes);