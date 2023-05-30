using YourBrand.Payments.Application.Features.Receipts.Dtos;
using YourBrand.Payments.Application.Features.Users;
using YourBrand.Payments.Domain.ValueObjects;

namespace YourBrand.Payments.Application;

public static class Mappings
{
    public static ReceiptDto ToDto(this Receipt order) => new ReceiptDto(order.Id, order.ReceiptNo, order.Date, order.Status.ToDto(), order.Assignee?.ToDto(), order.CustomerId, order.Currency,
        order.BillingDetails?.ToDto(),
        order.ShippingDetails?.ToDto(),
        order.Items.Select(x => x.ToDto()), order.SubTotal, order.Vat.GetValueOrDefault(), order.Total, order.Created, order.CreatedBy?.ToDto(), order.LastModified, order.LastModifiedBy?.ToDto());

    public static ReceiptItemDto ToDto(this ReceiptItem orderItem) => new(orderItem.Id, orderItem.Description, orderItem.ItemId, orderItem.Unit, orderItem.UnitPrice, orderItem.Quantity, orderItem.VatRate, orderItem.Total, orderItem.Notes, orderItem.Created, orderItem.CreatedBy?.ToDto(), orderItem.LastModified, orderItem.LastModifiedBy?.ToDto());

    public static ReceiptStatusDto ToDto(this ReceiptStatus orderStatus) => new(orderStatus.Id, orderStatus.Name);

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
    
    public static CurrencyAmountDto ToDto(this CurrencyAmount currencyAmount) => new(currencyAmount.Currency, currencyAmount.Amount);

}
