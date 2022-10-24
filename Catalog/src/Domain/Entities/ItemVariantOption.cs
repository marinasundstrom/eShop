namespace YourBrand.Catalog.Domain.Entities;

public class ItemVariantOption
{
    public int Id { get; set; }

    public string ItemId { get; set; } = null!;

    public Item Item{ get; set; } = null!;

    public string ItemVariantId { get; set; } = null!;

    public Item ItemVariant { get; set; } = null!;

    public string OptionId { get; set; } = null!;

    public Option Option { get; set; } = null!;

    public bool? IsSelected { get; set; }

    // Add fields for default values
}
