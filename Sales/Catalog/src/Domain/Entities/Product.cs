using YourBrand.Catalog.Domain.Enums;

namespace YourBrand.Catalog.Domain.Entities;

public sealed class Product : AggregateRoot<string>
{
    private Product() { }

    public Product(string id, string name) : base(id)
    {
        Name = name;
    }

    public Store Store { get; set; } = null!;

    public string? StoreId { get; private set; }

    public Brand Brand { get; set; } = null!;

    public string? BrandId { get; private set; }

    public string Name { get; set; } = null!;

    public string? Substitle { get; set; }

    //public bool IsNew { get; set; }

    public Product? ParentProduct { get; set; }

    public string? ParentProductId { get; set; }

    public ProductGroup? Group { get; set; }

    public ProductGroup? Group2 { get; set; }

    public ProductGroup? Group3 { get; set; }

    public string? Description { get; set; } = null!;

    public string? ManufacturerProductId { get; set; }

    public string? GTIN { get; set; }

    public string? Image { get; set; }

    public decimal? Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public int? QuantityAvailable { get; set; }

    public decimal? ShippingFee { get; set; }

    public bool HasVariants { get; set; } = false;

    public bool? AllCustom { get; set; }

    public List<Entities.Attribute> Attributes { get; } = new List<Entities.Attribute>();

    public List<ProductAttribute> ProductAttributes { get; } = new List<ProductAttribute>();

    public List<AttributeGroup> AttributeGroups { get; } = new List<AttributeGroup>();

    public List<Product> Variants { get; } = new List<Product>();

    public List<Option> Options { get; } = new List<Option>();

    public List<ProductOption> ProductOptions { get; } = new List<ProductOption>();

    public List<OptionGroup> OptionGroups { get; } = new List<OptionGroup>();

    public ProductVisibility Visibility { get; set; }

    public List<ProductAttributeValue> AttributeValues { get; } = new List<ProductAttributeValue>();

    public List<ProductVariantOption> ProductVariantOptions { get; } = new List<ProductVariantOption>();
}
