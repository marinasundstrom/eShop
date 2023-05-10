using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application;

public static class Mapper
{
    public static SiteProductDto ToDto(this YourBrand.Catalog.ProductDto product, Catalog.StoreDto store, string? description = null) =>
        new (product.Id, product.Name, product.Headline, product.Handle, !string.IsNullOrEmpty(description) ? description : product.Description, 
        product.Parent?.ToDto2(), product.Group?.ToDto(), product.Image, 
        product.Price?.GetCurrencyAmountDto(store), product.RegularPrice?.GetCurrencyAmountDto(store), product.QuantityAvailable, 
        product.Attributes, product.Options, product.IsCustomizable, product.HasVariants);

    public static SiteParentProductDto ToDto2(this YourBrand.Catalog.ParentProductDto product) =>
        new (product.Id, product.Name, product.Handle, product.Description, product.Group?.ToDto());

    public static SiteProductGroupDto ToDto(this YourBrand.Catalog.ProductGroupDto dto) => new (dto.Id, dto.Name, dto.Handle, dto.Path, dto.Parent?.ToDto2());

    public static SiteParentProductGroupDto ToDto2(this YourBrand.Catalog.ParentProductGroupDto dto) => new (dto.Id, dto.Name, dto.Handle, dto.Path, dto.Parent?.ToDto2());

    public static AddressDto ToDto(this YourBrand.Orders.AddressDto address) => new()
    {
        Thoroughfare = address.Thoroughfare,
        Premises = address.Premises,
        SubPremises = address.SubPremises,
        PostalCode = address.PostalCode,
        Locality = address.Locality,
        SubAdministrativeArea = address.SubAdministrativeArea,
        AdministrativeArea = address.AdministrativeArea,
        Country = address.Country
    };

    public static AddressDto ToDto(this YourBrand.Customers.AddressDto address) => new()
    {
        Thoroughfare = address.Thoroughfare,
        Premises = address.Premises,
        SubPremises = address.SubPremises,
        PostalCode = address.PostalCode,
        Locality = address.Locality,
        SubAdministrativeArea = address.SubAdministrativeArea,
        AdministrativeArea = address.AdministrativeArea,
        Country = address.Country
    };

    public static CurrencyAmountDto GetCurrencyAmountDto(this decimal price, Catalog.StoreDto store) => new(store.Currency.Code, price);
}

public record CurrencyAmountDto(string Currency, decimal Amount);