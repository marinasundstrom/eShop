namespace YourBrand.CustomerService.Application.Tickets.Dtos;

using YourBrand.CustomerService.Application.Users;

public sealed record TicketDto(int Id, string Title, string? Description, TicketStatusDto Status, UserDto? AssigneeId, double? EstimatedHours, double? RemainingHours, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
