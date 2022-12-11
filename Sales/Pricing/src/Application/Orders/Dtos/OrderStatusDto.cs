using System.ComponentModel.DataAnnotations;

using YourBrand.Pricing.Application;

namespace YourBrand.Pricing.Application.Orders.Dtos;

public record OrderStatusDto
(
    int Id,
    string Name
);
