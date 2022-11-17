using MediatR;
using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Services;

namespace YourBrand.CustomerService.Application.Tickets.EventHandlers;

public sealed class TicketDescriptionUpdatedEventHandler : IDomainEventHandler<TicketDescriptionUpdated>
{
    private readonly ITicketRepository ticketRepository;
    private readonly ITicketNotificationService ticketNotificationService;

    public TicketDescriptionUpdatedEventHandler(ITicketRepository ticketRepository, ITicketNotificationService ticketNotificationService)
    {
        this.ticketRepository = ticketRepository;
        this.ticketNotificationService = ticketNotificationService;
    }

    public async Task Handle(TicketDescriptionUpdated notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await ticketNotificationService.DescriptionUpdated(ticket.Id, ticket.Description);
    }
}

