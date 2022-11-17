using MediatR;
using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Services;
using YourBrand.CustomerService.Application.Tickets.Dtos;

namespace YourBrand.CustomerService.Application.Tickets.EventHandlers;

public sealed class TicketTitleUpdatedEventHandler : IDomainEventHandler<TicketTitleUpdated>
{
    private readonly ITicketRepository ticketRepository;
    private readonly ITicketNotificationService ticketNotificationService;

    public TicketTitleUpdatedEventHandler(ITicketRepository ticketRepository, ITicketNotificationService ticketNotificationService)
    {
        this.ticketRepository = ticketRepository;
        this.ticketNotificationService = ticketNotificationService;
    }

    public async Task Handle(TicketTitleUpdated notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await ticketNotificationService.TitleUpdated(ticket.Id, ticket.Title);
    }
}
