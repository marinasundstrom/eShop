using Microsoft.AspNetCore.SignalR;
using YourBrand.Sales.Application.Services;
using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Presentation.Hubs;

public class OrderNotificationService : IOrderNotificationService
{
    private readonly IHubContext<OrdersHub, IOrdersHubClient> hubsContext;

    public OrderNotificationService(IHubContext<OrdersHub, IOrdersHubClient> hubsContext)
    {
        this.hubsContext = hubsContext;
    }

    public async Task Created(string orderId)
    {
        await hubsContext.Clients.All.Created(orderId);
    }

    public async Task Updated(string orderId)
    {
        await hubsContext.Clients.All.Updated(orderId);
    }

    public async Task Deleted(string orderId)
    {
        await hubsContext.Clients.All.Deleted(orderId);
    }

    public async Task StatusUpdated(string orderId, OrderStatusDto status)
    {
        await hubsContext.Clients.All.StatusUpdated(orderId, status);
    }
}