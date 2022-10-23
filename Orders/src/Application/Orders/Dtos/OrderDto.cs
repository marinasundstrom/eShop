namespace YourBrand.Orders.Application.Orders.Dtos;

using YourBrand.Orders.Application.Users;

public sealed record OrderDto(int Id, OrderStatusDto Status, UserDto? AssignedTo, DateTimeOffset Created, UserDto CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
