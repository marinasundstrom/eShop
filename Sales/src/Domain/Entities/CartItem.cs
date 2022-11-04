namespace YourBrand.Sales.Domain.Entities;

public class CartItem : Entity<string>, IAuditable, ISoftDelete
{
    protected CartItem()
    {
    }

    internal CartItem(string? itemId, double quantity, string? data)
        : base(Guid.NewGuid().ToString())
    {
        ItemId = itemId;
        Quantity = quantity;
        Data = data;
    }

    public Cart? Cart { get; private set; }

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

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public string? DeletedById { get; set; }

    public DateTimeOffset? Deleted { get; set; }

    public DateTimeOffset? CheckedOutAt { get; set; }
}
