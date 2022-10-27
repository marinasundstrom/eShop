using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Services;

public interface IOrderNotificationService
{
    Task Created(int orderNo);

    Task Updated(int orderNo);

    Task Deleted(int orderNo);

    Task StatusUpdated(int orderNo, OrderStatusDto status);
}
