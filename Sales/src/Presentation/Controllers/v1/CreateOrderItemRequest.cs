namespace YourBrand.Sales.Presentation.Controllers;

public sealed record CreateOrderItemRequest(string Description, string? ItemId, decimal Price, double Quantity, decimal Total);