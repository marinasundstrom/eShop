namespace YourBrand.Catalog.Domain.Entities;

using System.ComponentModel.DataAnnotations.Schema;

using YourBrand.Catalog.Domain.Enums;

public class Option : Entity<string>
{
    protected Option() { }

    public Option(string name) 
        : base(Guid.NewGuid().ToString())
    {
        Name = name;
    }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public OptionGroup? Group { get; set; }

    public ItemGroup? ItemGroup { get; set; }

    public OptionType OptionType { get; set; } = OptionType.Choice;

    public bool IsRequired { get; set; }

    public bool IsSelected { get; set; }

    public string? ItemId { get; set; }

    public decimal? Price { get; set; }

    public List<OptionValue> Values { get; } = new List<OptionValue>();

    public List<Item> Items { get; } = new List<Item>();

    public List<ItemVariantOption> ItemVariantOptions { get; } = new List<ItemVariantOption>();

    [ForeignKey(nameof(DefaultValue))]
    public string? DefaultValueId { get; set; }

    public OptionValue? DefaultValue { get; set; }

    //public bool HasCustomData { get; set; }

    public int? MinNumericalValue  { get; set; }

    public int? MaxNumericalValue  { get; set; }

    public int? DefaultNumericalValue { get; set; }

    public int? TextValueMinLength  { get; set; }

    public int? TextValueMaxLength  { get; set; }

    public string? DefaultTextValue { get; set; }
}
