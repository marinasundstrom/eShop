using MediatR;
using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Services;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.EventHandlers;

public sealed class ReceiptStatusUpdatedEventHandler : IDomainEventHandler<ReceiptStatusUpdated>
{
    private readonly IReceiptRepository orderRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IEmailService emailService;
    private readonly IReceiptNotificationService orderNotificationService;

    public ReceiptStatusUpdatedEventHandler(IReceiptRepository orderRepository, ICurrentUserService currentUserService, IEmailService emailService, IReceiptNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.currentUserService = currentUserService;
        this.emailService = emailService;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(ReceiptStatusUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.ReceiptId, cancellationToken);

        if (order is null)
            return;

        //await orderNotificationService.StatusUpdated(order.ReceiptNo, 1);

        if (order.AssigneeId is not null && order.LastModifiedById != order.AssigneeId)
        {/*
            await emailService.SendEmail(order.AssigneeId!.Email,
                $"Status of \"{order.Title}\" [{order.ReceiptNo}] changed to {notification.NewStatus}.",
                $"{order.LastModifiedBy!.Name} changed status of \"{order.Title}\" [{order.ReceiptNo}] from {notification.OldStatus} to {notification.NewStatus}."); */
        }
    }
}
