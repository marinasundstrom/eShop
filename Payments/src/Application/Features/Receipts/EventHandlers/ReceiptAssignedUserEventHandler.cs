using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Services;

namespace YourBrand.Payments.Application.Features.Receipts.EventHandlers;

public sealed class ReceiptAssignedUserEventHandler : IDomainEventHandler<ReceiptAssignedUserUpdated>
{
    private readonly IReceiptRepository orderRepository;
    private readonly IEmailService emailService;
    private readonly IReceiptNotificationService orderNotificationService;

    public ReceiptAssignedUserEventHandler(IReceiptRepository orderRepository, IEmailService emailService, IReceiptNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.emailService = emailService;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(ReceiptAssignedUserUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.ReceiptId, cancellationToken);

        if (order is null)
            return;

        if (order.AssigneeId is not null && order.LastModifiedById != order.AssigneeId)
        {
            /*
            await emailService.SendEmail(
                order.AssigneeId!.Email,
                $"You were assigned to \"{order.Title}\" [{order.ReceiptNo}].",
                $"{order.LastModifiedBy!.Name} assigned {order.AssigneeId.Name} to \"{order.Title}\" [{order.ReceiptNo}]."); */
        }
    }
}
