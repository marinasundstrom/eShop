using YourBrand.Pricing.Application.Common;
using YourBrand.Pricing.Application.Services;
using YourBrand.Pricing.Domain.Entities;

namespace YourBrand.Pricing.Application.Orders.EventHandlers;

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

