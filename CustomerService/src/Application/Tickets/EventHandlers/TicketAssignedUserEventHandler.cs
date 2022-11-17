using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Services;

namespace YourBrand.CustomerService.Application.Tickets.EventHandlers;

public sealed class TicketAssignedUserEventHandler : IDomainEventHandler<TicketAssignedUserUpdated>
{
    private readonly ITicketRepository ticketRepository;
    private readonly IEmailService emailService;
    private readonly ITicketNotificationService ticketNotificationService;

    public TicketAssignedUserEventHandler(ITicketRepository ticketRepository, IEmailService emailService, ITicketNotificationService ticketNotificationService)
    {
        this.ticketRepository = ticketRepository;
        this.emailService = emailService;
        this.ticketNotificationService = ticketNotificationService;
    }

    public async Task Handle(TicketAssignedUserUpdated notification, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.FindByIdAsync(notification.TicketId, cancellationToken);

        if (ticket is null)
            return;

        if (ticket.AssigneeId is not null && ticket.LastModifiedById != ticket.AssigneeId)
        {
            await emailService.SendEmail(
                ticket.Assignee!.Email,
                $"You were assigned to \"{ticket.Title}\" [{ticket.Id}].",
                $"{ticket.LastModifiedBy!.Name} assigned {ticket.Assignee.Name} to \"{ticket.Title}\" [{ticket.Id}].");
        }
    }
}
