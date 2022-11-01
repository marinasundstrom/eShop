namespace YourBrand.Sales.Domain.Entities;

public class OrderItem : AuditableEntity
{
    protected OrderItem()
    {
    }

    internal OrderItem(string? itemId, string description, double quantity, string? unit, decimal unitPrice, decimal total, double vatRate)
    {
        ItemId = itemId;
        Description = description;
        Quantity = quantity;
        Unit = unit;
        UnitPrice = unitPrice;
        Total = total;
        VatRate = vatRate;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public string? ItemId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Quantity { get; set; }

    public string? Unit { get; set; }

    public decimal UnitPrice { get; set; }
    
    public decimal Total { get; set; }

    public double VatRate { get; set; }
}
