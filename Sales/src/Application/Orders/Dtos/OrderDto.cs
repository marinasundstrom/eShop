namespace YourBrand.Sales.Application.Orders.Dtos;

using YourBrand.Sales.Application.Users;

public sealed record OrderDto(string Id, OrderStatusDto Status, UserDto? AssignedTo, IEnumerable<OrderItemDto> Items, DateTimeOffset Created, UserDto CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record OrderItemDto(string Id, string Description, string? ItemId, decimal Price, double Quantity, decimal Total, DateTimeOffset Created, UserDto CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
