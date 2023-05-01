using System;
using Microsoft.Extensions.Logging;

namespace YourBrand.StoreFront.Application.Features.Carts;

public record AddCartItemDto(string? ProductId, int Quantity, string? Data);

public record UpdateCartItemDto(int Quantity, string? Data);

public record SiteProductDto(long Id, string Name, string Handle, string? Description, SiteParentProductDto? Parent, SiteProductGroupDto? Group, string? Image, CurrencyAmountDto? Price, CurrencyAmountDto? RegularPrice, int? Available, IEnumerable<YourBrand.Catalog.ProductAttributeDto> Attributes, IEnumerable<YourBrand.Catalog.ProductOptionDto> Options, bool IsCustomizable, bool HasVariants);

public record SiteParentProductDto(long Id, string Name, string Handle, string? Description, SiteProductGroupDto? Group);

public record SiteProductGroupDto(long Id, string Name, string Handle, string Path, SiteParentProductGroupDto? Parent);

public record SiteParentProductGroupDto(long Id, string Name, string Handle, string Path, SiteParentProductGroupDto? Parent);

public record SiteCartDto(string Id, IEnumerable<SiteCartItemDto> Items);

public record SiteCartItemDto(string Id, SiteProductDto Product, int Quantity, decimal Total, string? Data);

public class Option
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int OptionType { get; set; }
    
    public string? ProductId { get; set; }

    public decimal? Price { get; set; }

    public string? TextValue { get; set; }

    public int? NumericalValue { get; set; }

    public bool? IsSelected { get; set; }

    public string? SelectedValueId { get; set; }
}

