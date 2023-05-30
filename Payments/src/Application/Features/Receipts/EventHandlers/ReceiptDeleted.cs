using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Services;
using YourBrand.Payments.Domain.Entities;

namespace YourBrand.Payments.Application.Features.Receipts.EventHandlers;

public sealed class ReceiptDeletedEventHandler : IDomainEventHandler<ReceiptDeleted>
{
    private readonly IReceiptRepository orderRepository;
    private readonly IReceiptNotificationService orderNotificationService;

    public ReceiptDeletedEventHandler(IReceiptRepository orderRepository, IReceiptNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(ReceiptDeleted notification, CancellationToken cancellationToken)
    {
        await orderNotificationService.Deleted(notification.ReceiptNo);
    }
}

