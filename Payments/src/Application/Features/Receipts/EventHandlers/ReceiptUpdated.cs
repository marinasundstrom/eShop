using MediatR;
using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Services;

namespace YourBrand.Payments.Application.Features.Receipts.EventHandlers;

public sealed class ReceiptUpdatedEventHandler : IDomainEventHandler<ReceiptUpdated>
{
    private readonly IReceiptRepository orderRepository;
    private readonly IReceiptNotificationService orderNotificationService;

    public ReceiptUpdatedEventHandler(IReceiptRepository orderRepository, IReceiptNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(ReceiptUpdated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.ReceiptId, cancellationToken);

        if (order is null)
            return;

        //await orderNotificationService.Updated(order.ReceiptNo);
    }
}
