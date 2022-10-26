using YourBrand.Sales.Application.Carts.Dtos;
using YourBrand.Sales.Application.Orders.Dtos;
using YourBrand.Sales.Application.Users;

namespace YourBrand.Sales.Application;

public static class Mappings
{
    public static OrderDto ToDto(this Order order) => new OrderDto(order.Id, (OrderStatusDto)order.Status, order.Assignee?.ToDto(), order.Items.Select(x => x.ToDto()), order.Created, order.CreatedBy?.ToDto(), order.LastModified, order.LastModifiedBy?.ToDto());

    public static OrderItemDto ToDto(this OrderItem orderItem) => new OrderItemDto(orderItem.Id, orderItem.Description, orderItem.ItemId, orderItem.Price, orderItem.Quantity, orderItem.Total, orderItem.Created, orderItem.CreatedBy?.ToDto(), orderItem.LastModified, orderItem.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new UserDto(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new UserInfoDto(user.Id, user.Name);

    public static CartDto ToDto(this Cart cart) => new CartDto(cart.Id, cart.Items.Select(x => x.ToDto()), cart.Created, cart.CreatedBy?.ToDto(), cart.LastModified, cart.LastModifiedBy?.ToDto());

    public static CartItemDto ToDto(this CartItem cartItem) => new CartItemDto(cartItem.Id, cartItem.ItemId, cartItem.Quantity, cartItem.Created, cartItem.CreatedBy?.ToDto(), cartItem.LastModified, cartItem.LastModifiedBy?.ToDto());

}
