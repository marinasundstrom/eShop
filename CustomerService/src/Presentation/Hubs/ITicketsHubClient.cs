using YourBrand.CustomerService.Application.Tickets.Dtos;

namespace YourBrand.CustomerService.Presentation.Hubs;

public interface ITicketsHubClient
{
    Task Created(int todoId, string title);

    Task Updated(int todoId, string title);

    Task Deleted(int todoId, string title);

    Task TitleUpdated(int todoId, string title);

    Task DescriptionUpdated(int todoId, string? description);

    Task StatusUpdated(int todoId, TicketStatusDto status);

    Task EstimatedHoursUpdated(int todoId, double? hours);

    Task RemainingHoursUpdated(int todoId, double? hours);
}