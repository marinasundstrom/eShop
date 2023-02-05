using YourBrand.Orders.Application.Features.Orders.Dtos;

namespace YourBrand.Orders.Application.Features.Orders;

public interface IOrdersHubClient
{
    Task Created(int orderNo);

    Task Updated(int orderNo);

    Task Deleted(int orderNo);

    Task StatusUpdated(int orderNo, OrderStatusDto status);
}