namespace YourBrand.CustomerService.Application.Tickets.Dtos;

using YourBrand.CustomerService.Application.Users;

public sealed record TicketCommentDto(int Id, string Text, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
