using System;
using MediatR;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Services;

namespace YourBrand.Sales.Application.Orders.EventHandlers;

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
        var order = await orderRepository.FindByIdAsync(notification.OrderNo, cancellationToken);

        if (order is null)
            return;

        //await orderNotificationService.Created(order.OrderNo);
    }
}

