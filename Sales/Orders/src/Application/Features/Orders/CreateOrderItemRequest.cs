namespace YourBrand.Orders.Application.Features.Orders;

public sealed record CreateOrderItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, string? Notes);