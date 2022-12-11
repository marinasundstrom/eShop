using YourBrand.Orders.Domain.ValueObjects;

namespace YourBrand.Orders.Domain.Entities;


public class ProductPrice : Entity<string>, IAuditable
{
    protected ProductPrice()
        : base()
    {
    }

    public ProductPrice(string productPriceListId, string productId, CurrencyAmount price)
        : base(Guid.NewGuid().ToString())
    {
        ProductPriceListId = productPriceListId;
        ProductId = productId;
        Price = price;
    }

    public string ProductPriceListId { get; private set; } = null!;

    public string ProductId { get; private set; } = null!;

    public CurrencyAmount Price { get; private set; } = null!;

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
