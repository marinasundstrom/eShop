using YourBrand.CustomerService.Application.Tickets.Dtos;

namespace YourBrand.CustomerService.Application;

public static partial class Mappings
{
    public static TicketDto ToDto(this Ticket ticket) => new TicketDto(ticket.Id, ticket.Title, ticket.Description, (TicketStatusDto)ticket.Status, ticket.Assignee?.ToDto(), ticket.EstimatedHours, ticket.RemainingHours, ticket.Created, ticket.CreatedBy?.ToDto(), ticket.LastModified, ticket.LastModifiedBy?.ToDto());
}
