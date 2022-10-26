namespace YourBrand.Sales.Application.Orders.Dtos;

public sealed class CreateOrderItemDto
{
    public string Description { get; set; } = null!;

    public string? ItemId { get; set; }

    public decimal Price { get; set; }

    public double VatRate { get; set; }

    public double Quantity { get; set; }
}
