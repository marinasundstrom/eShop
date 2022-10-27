namespace YourBrand.Sales.Domain.Entities;

public class CartItem : AuditableEntity, IAggregateRoot
{
    protected CartItem()
    {
    }

    internal CartItem(string? itemId, double quantity, string? data)
    {
        ItemId = itemId;
        Quantity = quantity;
        Data = data;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public string? ItemId { get; private set; } = null!;

    public double Quantity { get; private set; }

    public string? Data { get; private set; }

    public void UpdateQuantity(double value) => Quantity = value;

    public void AddToQuantity(double value) => Quantity += value;

    public void RemoveQuantity(double value) => Quantity -= value;

    public void UpdateData(string? data)
    {
        Data = data;
    }
}
