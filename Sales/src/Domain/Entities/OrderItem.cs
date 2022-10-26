namespace YourBrand.Sales.Domain.Entities;

public class OrderItem : AuditableEntity, IAggregateRoot
{
    protected OrderItem()
    {
    }

    internal OrderItem(string description, string? itemId, string? unit, decimal unitPrice, double vatRate, double quantity, decimal total)
    {
        Description = description;
        ItemId = itemId;
        Unit = unit;
        UnitPrice = unitPrice;
        VatRate = vatRate;
        Quantity = quantity;
        Total = total;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public string Description { get; set; } = null!;

    public string? ItemId { get; set; } = null!;

    public string? Unit { get; set; }

    public decimal UnitPrice { get; set; }

    public double VatRate { get; set; }

    public double Quantity { get; set; }

    public decimal Total { get; set; }
}
