namespace YourBrand.Orders.Application.Features.Orders.Dtos;

using YourBrand.Orders.Application.Features.Users;

public sealed record OrderDto(string Id, int OrderNo, DateTime Date, OrderStatusDto Status, UserDto? AssigneeId, string? CustomerId, string Currency, BillingDetailsDto? BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<OrderItemDto> Items, decimal SubTotal, decimal Vat, decimal Total, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record OrderItemDto(string Id, string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, decimal Total, string? Notes, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
