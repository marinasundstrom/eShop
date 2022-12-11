namespace YourBrand.Subscriptions.Presentation.Controllers;

public sealed record CreateOrderItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, string? Notes);

public sealed record CreateProductPriceRequest(string ItemId, decimal Price);


public sealed record CreateProductPriceListRequest(string Name);