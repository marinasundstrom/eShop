using System.ComponentModel.DataAnnotations;

using YourBrand.Sales.Client;

namespace YourBrand.Sales.Orders;

public class OrderItemViewModel
{
    public string? Id { get; set; }

    [Required]
    public string Description { get; set; } = null!;

    public string? ItemId { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public string Unit { get; set; } = null!;

    [Required]
    [Range(0.0001, double.MaxValue)]
    public double Quantity { get; set; } = 1;

    public double VatRate { get; set; } = 0.25;

    public decimal LineTotal => UnitPrice * (decimal)Quantity;
}
