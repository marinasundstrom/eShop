using YourBrand.Orders.Application.Common;
using YourBrand.Orders.Application.Services;

namespace YourBrand.Orders.Application.Orders.EventHandlers;

public sealed class OrderAssignedUserEventHandler : IDomainEventHandler<OrderAssignedUserUpdated>
{
    private readonly IOrderRepository orderRepository;
    private readonly IEmailService emailService;
    private readonly IOrderNotificationService orderNotificationService;

    public OrderAssignedUserEventHandler(IOrderRepository orderRepository, IEmailService emailService, IOrderNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.emailService = emailService;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(OrderAssignedUserUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.OrderId, cancellationToken);

        if (order is null)
            return;

        if (order.AssignedToId is not null && order.LastModifiedById != order.AssignedToId)
        {
            /*
            await emailService.SendEmail(
                order.AssignedTo!.Email,
                $"You were assigned to \"{order.Title}\" [{order.Id}].",
                $"{order.LastModifiedBy!.Name} assigned {order.AssignedTo.Name} to \"{order.Title}\" [{order.Id}]."); */
        }
    }
}
