namespace YourBrand.Sales.Application.Carts.Dtos;

using YourBrand.Sales.Application.Users;

public sealed record CartDto(string Id, IEnumerable<CartItemDto> Items, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record CartItemDto(string Id, string? ItemId, double Quantity, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
