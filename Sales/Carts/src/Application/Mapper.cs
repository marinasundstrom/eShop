using YourBrand.Carts.Application.Carts.Dtos;
using YourBrand.Carts.Application.Users;
using YourBrand.Carts.Domain.ValueObjects;

namespace YourBrand.Carts.Application;

public static class Mappings
{
    public static CartDto ToDto(this Cart cart) => new(cart.Id, cart.Items.Select(x => x.ToDto()), cart.Created, cart.CreatedBy?.ToDto(), cart.LastModified, cart.LastModifiedBy?.ToDto());

    public static CartItemDto ToDto(this CartItem cartItem) => new(cartItem.Id, cartItem.ItemId, cartItem.Quantity, cartItem.Data, cartItem.Created, cartItem.CreatedBy?.ToDto(), cartItem.LastModified, cartItem.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new(user.Id, user.Name);
}
