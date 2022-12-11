using YourBrand.Orders.Application.Orders.Dtos;

namespace YourBrand.Orders.Presentation.Hubs;

public interface IOrdersHubClient
{
    Task Created(int orderNo);

    Task Updated(int orderNo);

    Task Deleted(int orderNo);

    Task StatusUpdated(int orderNo, OrderStatusDto status);
}