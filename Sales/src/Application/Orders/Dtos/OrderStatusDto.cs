using System.ComponentModel.DataAnnotations;

using YourBrand.Sales.Application;

namespace YourBrand.Sales.Application.Orders.Dtos;

public record OrderStatusDto
(
    int Id,
    string Name
);
