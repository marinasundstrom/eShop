using YourBrand.Orders.Application.Orders.Dtos;

namespace YourBrand.Orders.Presentation.Controllers;

public sealed record CreateOrderRequest(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items);
