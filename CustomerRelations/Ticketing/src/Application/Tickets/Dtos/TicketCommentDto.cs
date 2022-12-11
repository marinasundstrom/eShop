namespace YourBrand.Ticketing.Application.Tickets.Dtos;

using YourBrand.Ticketing.Application.Users;

public sealed record TicketCommentDto(int Id, string Text, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
