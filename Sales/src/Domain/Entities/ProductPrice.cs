namespace YourBrand.Sales.Domain.Entities;

public class ProductPrice : AggregateRoot<string>, IAuditable
{
    public ProductPrice(string id, string productId, decimal price)
        : base(id)
    {
        Id = id;
        ProductId = productId;
        Price = price;
    }

    public string ProductId { get; private set; }

    public decimal Price { get; private set; }

    public decimal? CompareAtPrice { get; private set; }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
