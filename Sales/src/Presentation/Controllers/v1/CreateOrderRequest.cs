using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Presentation.Controllers;

public sealed record CreateOrderRequest(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items);
