using YourBrand.Orders.Application.Orders.Dtos;
using YourBrand.Orders.Application.Users;

namespace YourBrand.Orders.Application;

public static class Mappings
{
    public static OrderDto ToDto(this Order order) => new OrderDto(order.Id, (OrderStatusDto)order.Status, order.AssignedTo?.ToDto(), order.Created, order.CreatedBy.ToDto(), order.LastModified, order.LastModifiedBy?.ToDto());

    public static UserDto ToDto(this User user) => new UserDto(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new UserInfoDto(user.Id, user.Name);
}
