namespace YourBrand.Marketing.Domain.Entities;

public class Discount : Entity<string>, IAuditable
{

#nullable disable

    protected Discount()
    {

    }

#nullable restore

    public Discount(string productId, string productName, string? productDescription, decimal ordinaryPrice, double percent)
    : base(Guid.NewGuid().ToString())
    {
        ProductId = productId;
        ProductName = productName;
        ProductDescription1 = productDescription;
        OrdinaryPrice = ordinaryPrice;
        Percent = percent;
    }

    public Contact? Contact { get; set; }

    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; }
    public string? ProductDescription1 { get; }
    public decimal? ProductDescription { get; set; }

    public double Percent { get; set; }
    public decimal OrdinaryPrice { get; set; }
    public decimal DiscountedPrice { get; set; }

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}