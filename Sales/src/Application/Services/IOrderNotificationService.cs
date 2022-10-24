using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Services;

public interface IOrderNotificationService
{
    Task Created(string orderId);

    Task Updated(string orderId);

    Task Deleted(string orderId);

    Task StatusUpdated(string orderId, OrderStatusDto status);
}
