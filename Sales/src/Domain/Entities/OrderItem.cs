namespace YourBrand.Sales.Domain.Entities;

public class OrderItem : Entity<string>, IAuditable
{
    internal OrderItem(string? itemId, string description, double quantity, string? unit, decimal unitPrice, decimal total, double vatRate, string? notes)
        : base(Guid.NewGuid().ToString()) 
    {
        ItemId = itemId;
        Description = description;
        Quantity = quantity;
        Unit = unit;
        UnitPrice = unitPrice;
        Total = total;
        VatRate = vatRate;
        Notes = notes;
    }

    public Order? Order { get; private set; }

    public string? ItemId { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Quantity { get; set; }

    public string? Unit { get; set; }

    public decimal UnitPrice { get; set; }
    
    public decimal Total { get; set; }

    public double VatRate { get; set; }

    public string? Notes { get; set; }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
