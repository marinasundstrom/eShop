using YourBrand.Orders.Application.Dtos;
using YourBrand.Shops.Application.Users;
using YourBrand.Shops.Domain.ValueObjects;

namespace YourBrand.Shops.Application;

public static class Mappings
{
    public static ShopDto ToDto(this Shop shop) => new(shop.Id, shop.Name, shop.Created, shop.CreatedBy?.ToDto(), shop.LastModified, shop.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new(user.Id, user.Name);
}
