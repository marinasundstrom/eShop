using YourBrand.Subscriptions.Application.Orders.Dtos;

namespace YourBrand.Subscriptions.Presentation.Controllers;

public sealed record CreateOrderRequest(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items);
