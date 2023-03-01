namespace YourBrand.Portal.Todos.Dtos;

using YourBrand.Portal.Users;

public sealed record TodoDto(int Id, string Title, string? Description, TodoStatusDto Status, UserDto? AssigneeId, double? EstimatedHours, double? RemainingHours, DateTimeOffset Created, UserDto CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
