using YourBrand.Orders.Application.Orders.Dtos;

namespace YourBrand.Orders.Application.Services;

public interface IOrderNotificationService
{
    Task Created(int orderId);

    Task Updated(int orderId);

    Task Deleted(int orderId);

    Task StatusUpdated(int orderId, OrderStatusDto status);
}
