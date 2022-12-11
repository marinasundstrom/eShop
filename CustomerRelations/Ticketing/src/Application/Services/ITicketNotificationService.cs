using YourBrand.Ticketing.Application.Tickets.Dtos;

namespace YourBrand.Ticketing.Application.Services;

public interface ITicketNotificationService
{
    Task Created(int ticketId, string title);

    Task Updated(int ticketId, string title);

    Task Deleted(int ticketId, string title);

    Task TitleUpdated(int ticketId, string title);

    Task DescriptionUpdated(int ticketId, string? description);

    Task StatusUpdated(int ticketId, TicketStatusDto status);

    Task EstimatedHoursUpdated(int ticketId, double? hours);

    Task RemainingHoursUpdated(int ticketId, double? hours);
}
