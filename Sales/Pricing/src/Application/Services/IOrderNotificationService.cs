using YourBrand.Pricing.Application.Orders.Dtos;

namespace YourBrand.Pricing.Application.Services;

public interface IOrderNotificationService
{
    Task Created(int orderNo);

    Task Updated(int orderNo);

    Task Deleted(int orderNo);

    Task StatusUpdated(int orderNo, OrderStatusDto status);
}
