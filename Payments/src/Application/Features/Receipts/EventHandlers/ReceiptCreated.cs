using System;
using MediatR;
using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Services;

namespace YourBrand.Payments.Application.Features.Receipts.EventHandlers;

public sealed class ReceiptCreatedEventHandler : IDomainEventHandler<ReceiptCreated>
{
    private readonly IReceiptRepository orderRepository;
    private readonly IReceiptNotificationService orderNotificationService;

    public ReceiptCreatedEventHandler(IReceiptRepository orderRepository, IReceiptNotificationService orderNotificationService)
    {
        this.orderRepository = orderRepository;
        this.orderNotificationService = orderNotificationService;
    }

    public async Task Handle(ReceiptCreated notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.FindByIdAsync(notification.ReceiptId, cancellationToken);

        if (order is null)
            return;

        //await orderNotificationService.Created(order.ReceiptNo);
    }
}

