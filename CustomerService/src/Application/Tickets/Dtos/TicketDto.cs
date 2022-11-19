namespace YourBrand.CustomerService.Application.Tickets.Dtos;

using YourBrand.CustomerService.Application.Users;

public sealed record TicketDto(int Id, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
