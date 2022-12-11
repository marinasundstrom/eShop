using YourBrand.Pricing.Application.Orders.Dtos;

namespace YourBrand.Pricing.Presentation.Controllers;

public sealed record CreateOrderRequest(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items);
