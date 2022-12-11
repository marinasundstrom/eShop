using System.ComponentModel.DataAnnotations;

using YourBrand.Subscriptions.Application;

namespace YourBrand.Subscriptions.Application.Orders.Dtos;

public record OrderStatusDto
(
    int Id,
    string Name
);
