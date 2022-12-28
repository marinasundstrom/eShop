using System;
using YourBrand.Shops.Application.Users;

namespace YourBrand.Orders.Application.Dtos;

public record ShopDto(string Id, string Name, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);