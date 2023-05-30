using Microsoft.AspNetCore.SignalR;
using YourBrand.Payments.Application.Services;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts;

public class ReceiptNotificationService : IReceiptNotificationService
{
    private readonly IHubContext<ReceiptsHub, IReceiptsHubClient> hubsContext;

    public ReceiptNotificationService(IHubContext<ReceiptsHub, IReceiptsHubClient> hubsContext)
    {
        this.hubsContext = hubsContext;
    }

    public async Task Created(int orderNo)
    {
        await hubsContext.Clients.All.Created(orderNo);
    }

    public async Task Updated(int orderNo)
    {
        await hubsContext.Clients.All.Updated(orderNo);
    }

    public async Task Deleted(int orderNo)
    {
        await hubsContext.Clients.All.Deleted(orderNo);
    }

    public async Task StatusUpdated(int orderNo, ReceiptStatusDto status)
    {
        await hubsContext.Clients.All.StatusUpdated(orderNo, status);
    }
}