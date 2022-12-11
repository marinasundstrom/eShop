using System;
using MediatR;
using YourBrand.Subscriptions.Application.Common;
using YourBrand.Subscriptions.Application.Services;

namespace YourBrand.Subscriptions.Application.Orders.EventHandlers;

public sealed class OrderCreatedEventHandler : IDomainEventHandler<OrderCreated>
{
    private readonly IOrderRepository orderRepository;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderCreatedEventHandler(IOrderRepository orderRepository, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
            return;

        //await orderNotificationService.Created(order.OrderNo);
    }
}

