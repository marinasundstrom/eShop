using MediatR;
using YourBrand.Orders.Application.Common;
using YourBrand.Orders.Application.Services;

namespace YourBrand.Orders.Application.Features.Orders.EventHandlers;

public sealed class OrderUpdatedEventHandler : IDomainEventHandler<OrderUpdated>
{
    private readonly IOrderRepository orderRepository;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderUpdatedEventHandler(IOrderRepository orderRepository, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
            return;

        //await orderNotificationService.Updated(order.OrderNo);
    }
}
