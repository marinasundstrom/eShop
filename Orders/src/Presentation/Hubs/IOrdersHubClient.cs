using YourBrand.Orders.Application.Orders.Dtos;

namespace YourBrand.Orders.Presentation.Hubs;

public interface IOrdersHubClient
{
    Task Created(int orderId);

    Task Updated(int orderId);

    Task Deleted(int orderId);

    Task StatusUpdated(int orderId, OrderStatusDto status);
}