using YourBrand.Orders.Application.Common;
using YourBrand.Orders.Application.Services;
using YourBrand.Orders.Domain.Entities;

namespace YourBrand.Orders.Application.Orders.EventHandlers;

public sealed class OrderDeletedEventHandler : IDomainEventHandler<OrderDeleted>
{
    private readonly IOrderRepository orderRepository;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderDeletedEventHandler(IOrderRepository orderRepository, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderDeleted notification, CancellationToken cancellationToken)
    {
        await orderNotificationService.Deleted(notification.OrderNo);
    }
}

