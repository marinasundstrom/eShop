using System;
using Microsoft.Extensions.Logging;

namespace YourBrand.StoreFront.Application.Carts;

public record AddCartItemDto(string? ItemId, int Quantity, string? Data);

public record UpdateCartItemDto(int Quantity, string? Data);

public record SiteItemDto(string Id, string Name, string? Description, SiteParentItemDto? Parent, SiteItemGroupDto? Group, string? Image, decimal Price, decimal? CompareAtPrice, int? Available, IEnumerable<YourBrand.Catalog.AttributeDto> Attributes, IEnumerable<YourBrand.Catalog.OptionDto> Options, bool HasVariants, IEnumerable<YourBrand.Catalog.ItemVariantAttributeDto> VariantAttributes);

public record SiteParentItemDto(string Id, string Name, string? Description, SiteItemGroupDto? Group);

public record SiteItemGroupDto(string Id, string Name, SiteItemGroupDto? Parent);

public record SiteCartDto(string Id, IEnumerable<SiteCartItemDto> Items);

public record SiteCartItemDto(string Id, SiteItemDto Item, int Quantity, decimal Total, string? Data);

public class Option
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int OptionType { get; set; }

    public string? ItemId { get; set; }

    public decimal? Price { get; set; }

    public string? TextValue { get; set; }

    public int? NumericalValue { get; set; }

    public bool? IsSelected { get; set; }

    public string? SelectedValueId { get; set; }
}

