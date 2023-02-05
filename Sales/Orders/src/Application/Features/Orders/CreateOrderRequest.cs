using YourBrand.Orders.Application.Features.Orders.Dtos;

namespace YourBrand.Orders.Application.Features.Orders;

public sealed record CreateOrderRequest(string? CustomerId, BillingDetailsDto BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<CreateOrderItemDto> Items);
