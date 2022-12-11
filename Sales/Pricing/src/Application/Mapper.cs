using YourBrand.Pricing.Application.Orders.Dtos;
using YourBrand.Pricing.Application.ProductPriceLists.Dtos;
using YourBrand.Pricing.Application.Users;
using YourBrand.Pricing.Domain.ValueObjects;

namespace YourBrand.Pricing.Application;

public static class Mappings
{
    public static OrderDto ToDto(this Order order) => new OrderDto(order.Id, order.OrderNo, order.Date, order.Status.ToDto(), order.Assignee?.ToDto(), order.CustomerId, order.Currency,
        order.BillingDetails?.ToDto(),
        order.ShippingDetails?.ToDto(),
        order.Items.Select(x => x.ToDto()), order.SubTotal, order.Vat.GetValueOrDefault(), order.Total, order.Created, order.CreatedBy?.ToDto(), order.LastModified, order.LastModifiedBy?.ToDto());

    public static OrderItemDto ToDto(this OrderItem orderItem) => new(orderItem.Id, orderItem.Description, orderItem.ItemId, orderItem.Unit, orderItem.UnitPrice, orderItem.Quantity, orderItem.VatRate, orderItem.Total, orderItem.Notes, orderItem.Created, orderItem.CreatedBy?.ToDto(), orderItem.LastModified, orderItem.LastModifiedBy?.ToDto());

    public static OrderStatusDto ToDto(this OrderStatus orderStatus) => new(orderStatus.Id, orderStatus.Name);

    public static UserDto ToDto(this User user) => new(user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new(user.Id, user.Name);

    public static AddressDto ToDto(this Domain.ValueObjects.Address address) => new()
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

    public static BillingDetailsDto ToDto(this BillingDetails billingDetails) => new()
    {
        FirstName = billingDetails.FirstName,
        LastName = billingDetails.LastName,
        SSN = billingDetails.SSN,
        Email = billingDetails.Email,
        PhoneNumber = billingDetails.PhoneNumber,
        Address = billingDetails.Address.ToDto()
    };

    public static ShippingDetailsDto ToDto(this ShippingDetails billingDetails) => new()
    {
        FirstName = billingDetails.FirstName,
        LastName = billingDetails.LastName,
        Address = billingDetails.Address.ToDto()
    };

    public static ProductPriceListDto ToDto(this ProductPriceList productPriceList) => new (productPriceList.Id,
        productPriceList.ProductPrices.Select(x => x.ToDto()), productPriceList.Created, productPriceList.CreatedBy?.ToDto(), productPriceList.LastModified, productPriceList.LastModifiedBy?.ToDto());

    public static ProductPriceDto ToDto(this ProductPrice productPrice) => new(productPrice.Id, productPrice.ProductId, productPrice.Price.ToDto(), productPrice.Created, productPrice.CreatedBy?.ToDto(), productPrice.LastModified, productPrice.LastModifiedBy?.ToDto());

    public static CurrencyAmountDto ToDto(this CurrencyAmount currencyAmount) => new(currencyAmount.Currency, currencyAmount.Amount);

}
