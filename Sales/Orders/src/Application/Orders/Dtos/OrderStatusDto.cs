using System.ComponentModel.DataAnnotations;

using YourBrand.Orders.Application;

namespace YourBrand.Orders.Application.Orders.Dtos;

public record OrderStatusDto
(
    int Id,
    string Name
);
