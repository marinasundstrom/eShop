namespace YourBrand.Catalog.Domain.Entities;

public class ProductVariant
{
    public string Id { get; set; } = null!;

    public Product Product { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Substitle { get; set; }

    public string? Description { get; set; }

    public string? ItemId { get; set; }

    public string? ManufacturerItemId { get; set; }

    public string? GTIN { get; set; }

    public string? Image { get; set; }

    public decimal? Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public decimal? ShippingFee { get; set; }

    public List<ProductVariantAttributeValue> AttributeValues { get; } = new List<ProductVariantAttributeValue>();

    public List<ProductVariantOption> ProductVariantOptions { get; } = new List<ProductVariantOption>();
}
