using YourBrand.Portal.Todos.Dtos;

namespace YourBrand.Portal.Controllers;

public sealed record CreateTodoRequest(string Title, string? Description, TodoStatusDto Status, string? AssigneeId, double? EstimatedHours, double? RemainingHours);
