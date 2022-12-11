namespace YourBrand.Orders.Domain.Entities;

public class ProductPriceList : AggregateRoot<string>, IAuditable
{
    private HashSet<ProductPrice> _productPrices = new HashSet<ProductPrice>();

    public ProductPriceList(string name)
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public string Name { get; private set; }

    public IReadOnlyCollection<ProductPrice> ProductPrices => _productPrices;

    public void AddProductPrice(ProductPrice productOffer) => _productPrices.Add(productOffer);

    public bool RemoveProductPrice(ProductPrice productOffer) => _productPrices.Remove(productOffer);

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
