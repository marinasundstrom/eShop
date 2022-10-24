using MediatR;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Services;
using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Orders.EventHandlers;

public sealed class OrderStatusUpdatedEventHandler : IDomainEventHandler<OrderStatusUpdated>
{
    private readonly IOrderRepository orderRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IEmailService emailService;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderStatusUpdatedEventHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService, IEmailService emailService, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.currentUserService = currentUserService;
        this.emailService = emailService;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderStatusUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
            return;

        await orderNotificationService.StatusUpdated(order.Id, (OrderStatusDto)order.Status);

        if (order.AssignedToId is not null && order.LastModifiedById != order.AssignedToId)
        {/*
            await emailService.SendEmail(order.AssignedTo!.Email,
                $"Status of \"{order.Title}\" [{order.Id}] changed to {notification.NewStatus}.",
                $"{order.LastModifiedBy!.Name} changed status of \"{order.Title}\" [{order.Id}] from {notification.OldStatus} to {notification.NewStatus}."); */
        }
    }
}
