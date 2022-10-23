using YourBrand.Orders.Application.Orders.Dtos;

namespace YourBrand.Orders.Presentation.Controllers;

public sealed record CreateOrderRequest(string Title, string? Description, OrderStatusDto Status, string? AssignedTo, double? EstimatedHours, double? RemainingHours);
