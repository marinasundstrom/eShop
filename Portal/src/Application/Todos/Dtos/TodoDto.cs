namespace YourBrand.Portal.Application.Todos.Dtos;

using YourBrand.Portal.Application.Users;

public sealed record TodoDto(int Id, string Title, string? Description, TodoStatusDto Status, UserDto? AssignedTo, double? EstimatedHours, double? RemainingHours, DateTimeOffset Created, UserDto CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
