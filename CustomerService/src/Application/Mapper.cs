using YourBrand.CustomerService.Application.Users;
using YourBrand.CustomerService.Application.Tickets.Dtos;
using YourBrand.CustomerService.Application.Users;
using YourBrand.CustomerService.Domain.Enums;
using YourBrand.CustomerService.Domain.ValueObjects;

namespace YourBrand.CustomerService.Application;

public static partial class Mappings
{
    public static TicketDto ToDto(this Ticket ticket) => new TicketDto(ticket.Id, ticket.Created, ticket.CreatedBy?.ToDto(), ticket.LastModified, ticket.LastModifiedBy?.ToDto());

    public static TicketCommentDto ToDto(this TicketComment ticketComment) => new TicketCommentDto(ticketComment.Id, ticketComment.Text, ticketComment.Created, ticketComment.CreatedBy?.ToDto(), ticketComment.LastModified, ticketComment.LastModifiedBy?.ToDto());

    public static TicketTypeDto ToDto(this TicketType ticketType) => new TicketTypeDto(ticketType.Id, ticketType.Name);

    public static TicketPriorityDto ToDto(this TicketPriority priority) => (TicketPriorityDto)priority;

    public static TicketSeverityDto ToDto(this TicketSeverity severity) => (TicketSeverityDto)severity;

    public static TicketStatusDto ToDto(this TicketStatus ticketStatus) => new TicketStatusDto(ticketStatus.Id, ticketStatus.Name);

    public static AttachmentDto ToDto(this Attachment attachment) => new AttachmentDto(attachment.Id, attachment.Name);

    public static TagDto ToDto(this Tag tag) => new TagDto(tag.Id, tag.Name);

    public static UserDto ToDto(this User user) => new(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new(user.Id, user.Name);
}
