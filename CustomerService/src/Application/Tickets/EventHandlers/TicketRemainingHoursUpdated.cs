using MediatR;
using YourBrand.CustomerService.Application.Common;

namespace YourBrand.CustomerService.Application.Tickets.EventHandlers;

public sealed class TicketRemainingHoursUpdatedEventHandler : IDomainEventHandler<TicketRemainingHoursUpdated>
{
    private readonly ITicketRepository ticketRepository;
    private readonly ITicketNotificationService ticketNotificationService;

    public TicketRemainingHoursUpdatedEventHandler(ITicketRepository ticketRepository, ITicketNotificationService ticketNotificationService)
    {
        this.ticketRepository = ticketRepository;
        this.ticketNotificationService = ticketNotificationService;
    }

    public async Task Handle(TicketRemainingHoursUpdated notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await ticketNotificationService.RemainingHoursUpdated(ticket.Id, ticket.RemainingHours);
    }
}