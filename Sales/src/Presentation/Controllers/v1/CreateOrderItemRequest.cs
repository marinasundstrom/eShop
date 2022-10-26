namespace YourBrand.Sales.Presentation.Controllers;

public sealed record CreateOrderItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate);