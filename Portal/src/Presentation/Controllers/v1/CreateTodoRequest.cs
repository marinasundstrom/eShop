using YourBrand.Portal.Application.Todos.Dtos;

namespace YourBrand.Portal.Presentation.Controllers;

public sealed record CreateTodoRequest(string Title, string? Description, TodoStatusDto Status, string? AssignedTo, double? EstimatedHours, double? RemainingHours);
