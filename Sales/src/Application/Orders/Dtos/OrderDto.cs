namespace YourBrand.Sales.Application.Orders.Dtos;

using YourBrand.Sales.Application.Users;

public sealed record OrderDto(string Id, DateTime Date, OrderStatusDto Status, UserDto? AssigneeId, string? CustomerId, string Currency, BillingDetailsDto? BillingDetails, ShippingDetailsDto? ShippingDetails, IEnumerable<OrderItemDto> Items, decimal SubTotal, decimal Vat, decimal Total, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record OrderItemDto(string Id, string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, decimal Total, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
