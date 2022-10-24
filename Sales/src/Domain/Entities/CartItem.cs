namespace YourBrand.Sales.Domain.Entities;

public class CartItem : AuditableEntity, IAggregateRoot
{
    protected CartItem()
    {
    }

    internal CartItem(string description, string? itemId, decimal price, double quantity, decimal total)
    {
        Description = description;
        ItemId = itemId;
        Price = price;
        Quantity = quantity;
        Total = total;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string Description { get; private set; } = null!;
    public string? ItemId { get; private set; } = null!;
    public decimal Price { get; private set; }
    public double Quantity { get; private set; }
    public decimal Total { get; private set; }
}
