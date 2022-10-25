using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Presentation.Controllers;

public sealed record CreateOrderRequest(string Title, string? Description, OrderStatusDto Status, string? AssigneeId, double? EstimatedHours, double? RemainingHours);
