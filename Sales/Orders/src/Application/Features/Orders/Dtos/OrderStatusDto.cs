using System.ComponentModel.DataAnnotations;

using YourBrand.Orders.Application;

namespace YourBrand.Orders.Application.Features.Orders.Dtos;

public record OrderStatusDto
(
    int Id,
    string Name,
    string Handle,
    string? Description
);
