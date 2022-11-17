using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Services;
using YourBrand.CustomerService.Domain.Entities;

namespace YourBrand.CustomerService.Application.Tickets.EventHandlers;

public sealed class TicketDeletedEventHandler : IDomainEventHandler<TicketDeleted>
{
    private readonly ITicketRepository ticketRepository;
    private readonly ITicketNotificationService ticketNotificationService;

    public TicketDeletedEventHandler(ITicketRepository ticketRepository, ITicketNotificationService ticketNotificationService)
    {
        this.ticketRepository = ticketRepository;
        this.ticketNotificationService = ticketNotificationService;
    }

    public async Task Handle(TicketDeleted notification, CancellationToken cancellationToken)
    {
        await ticketNotificationService.Deleted(notification.TicketId, notification.Title);
    }
}

