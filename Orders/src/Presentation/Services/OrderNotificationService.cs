using Microsoft.AspNetCore.SignalR;
using YourBrand.Orders.Application.Services;
using YourBrand.Orders.Application.Orders.Dtos;

namespace YourBrand.Orders.Presentation.Hubs;

public class OrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<OrdersHub, IOrdersHubClient> hubsContext;

    public OrderNotificationService(IHubContext<OrdersHub, IOrdersHubClient> hubsContext)
    {
        this.hubsContext = hubsContext;
    }

    public async Task Created(int orderId)
    {
        await hubsContext.Clients.All.Created(orderId);
    }

    public async Task Updated(int orderId)
    {
        await hubsContext.Clients.All.Updated(orderId);
    }

    public async Task Deleted(int orderId)
    {
        await hubsContext.Clients.All.Deleted(orderId);
    }

    public async Task StatusUpdated(int orderId, OrderStatusDto status)
    {
        await hubsContext.Clients.All.StatusUpdated(orderId, status);
    }
}