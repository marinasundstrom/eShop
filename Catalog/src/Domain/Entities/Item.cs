using YourBrand.Catalog.Domain.Enums;

namespace YourBrand.Catalog.Domain.Entities;

public class Item : IAggregateRoot
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Substitle { get; set; }

    public Item? ParentItem { get; set; }

    public string? ParentItemId { get; set; }

    public ItemGroup? Group { get; set; }

    public string? Description { get; set; } = null!;

    public string? ManufacturerItemId { get; set; }

    public string? GTIN { get; set; }

    public string? Image { get; set; }

    public decimal? Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public decimal? ShippingFee { get; set; }

    public bool HasVariants { get; set; } = false;

    public bool? AllCustom { get; set; }

    public List<Entities.Attribute> Attributes { get; } = new List<Entities.Attribute>();

    public List<ItemAttribute> ItemAttributes { get; } = new List<ItemAttribute>();

    public List<AttributeGroup> AttributeGroups { get; } = new List<AttributeGroup>();

    public List<Item> Variants { get; } = new List<Item>();

    public List<Option> Options { get; } = new List<Option>();

    public List<ItemOption> ItemOptions { get; } = new List<ItemOption>();

    public List<OptionGroup> OptionGroups { get; } = new List<OptionGroup>();

    public ItemVisibility Visibility { get; set; }

    public List<ItemAttributeValue> AttributeValues { get; } = new List<ItemAttributeValue>();

    public List<ItemVariantOption> ItemVariantOptions { get; } = new List<ItemVariantOption>();
}
