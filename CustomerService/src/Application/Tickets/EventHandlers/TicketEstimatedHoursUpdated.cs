using MediatR;
using YourBrand.CustomerService.Application.Common;

namespace YourBrand.CustomerService.Application.Tickets.EventHandlers;

public sealed class TicketEstimatedHoursUpdatedEventHandler : IDomainEventHandler<TicketEstimatedHoursUpdated>
{
    private readonly ITicketRepository ticketRepository;
    private readonly ITicketNotificationService ticketNotificationService;

    public TicketEstimatedHoursUpdatedEventHandler(ITicketRepository ticketRepository, ITicketNotificationService ticketNotificationService)
    {
        this.ticketRepository = ticketRepository;
        this.ticketNotificationService = ticketNotificationService;
    }

    public async Task Handle(TicketEstimatedHoursUpdated notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);

        if (ticket is null)
            return;

        await ticketNotificationService.EstimatedHoursUpdated(ticket.Id, ticket.EstimatedHours);
    }
}
