namespace YourBrand.Carts.Application.Features.Carts.Dtos;

using YourBrand.Carts.Application.Features.Users;

public sealed record CartDto(string Id, IEnumerable<CartItemDto> Items, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record CartItemDto(string Id, string? ItemId, double Quantity, string? Data, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
