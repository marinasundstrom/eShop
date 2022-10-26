namespace YourBrand.Sales.Domain.Entities;

public class OrderItem : AuditableEntity, IAggregateRoot
{
    protected OrderItem()
    {
    }

    internal OrderItem(string description, string? itemId, decimal price, double vatRate, double quantity, decimal total)
    {
        Description = description;
        ItemId = itemId;
        Price = price;
        VatRate = vatRate;
        Quantity = quantity;
        Total = total;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string Description { get; private set; } = null!;
    public string? ItemId { get; private set; } = null!;
    public decimal Price { get; private set; }
    public double VatRate { get; private set; }
    public double Quantity { get; private set; }
    public decimal Total { get; private set; }
}
