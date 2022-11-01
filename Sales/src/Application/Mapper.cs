using YourBrand.Sales.Application.Carts.Dtos;
using YourBrand.Sales.Application.Orders.Dtos;
using YourBrand.Sales.Application.Users;
using YourBrand.Sales.Domain.ValueObjects;

namespace YourBrand.Sales.Application;

public static class Mappings
{
    public static OrderDto ToDto(this Order order) => new OrderDto(order.OrderNo, order.Date, order.Status.ToDto(), order.Assignee?.ToDto(), order.CustomerId, order.Currency,
        order.BillingDetails?.ToDto(),
        order.ShippingDetails?.ToDto(),
        order.Items.Select(x => x.ToDto()), order.SubTotal, order.Vat.GetValueOrDefault(), order.Total, order.Created, order.CreatedBy?.ToDto(), order.LastModified, order.LastModifiedBy?.ToDto());

    public static OrderItemDto ToDto(this OrderItem orderItem) => new (orderItem.Id, orderItem.Description, orderItem.ItemId, orderItem.Unit, orderItem.UnitPrice, orderItem.Quantity, orderItem.VatRate, orderItem.Total, orderItem.Created, orderItem.CreatedBy?.ToDto(), orderItem.LastModified, orderItem.LastModifiedBy?.ToDto());

    public static OrderStatusDto ToDto(this OrderStatus orderStatus) => new (orderStatus.Id, orderStatus.Name);

    public static UserDto ToDto(this User user) => new (user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new (user.Id, user.Name);

    public static CartDto ToDto(this Cart cart) => new (cart.Id, cart.Items.Select(x => x.ToDto()), cart.Created, cart.CreatedBy?.ToDto(), cart.LastModified, cart.LastModifiedBy?.ToDto());

    public static CartItemDto ToDto(this CartItem cartItem) => new (cartItem.Id, cartItem.ItemId, cartItem.Quantity, cartItem.Data, cartItem.Created, cartItem.CreatedBy?.ToDto(), cartItem.LastModified, cartItem.LastModifiedBy?.ToDto());

    public static AddressDto ToDto(this Domain.ValueObjects.Address address) => new ()
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

    public static BillingDetailsDto ToDto(this BillingDetails billingDetails) => new () {
        FirstName = billingDetails.FirstName,
        LastName = billingDetails.LastName,
        SSN = billingDetails.SSN,
        Email = billingDetails.Email,
        PhoneNumber = billingDetails.PhoneNumber,
        Address = billingDetails.Address.ToDto()
    };

    public static ShippingDetailsDto ToDto(this ShippingDetails billingDetails) => new ()
    {
        FirstName = billingDetails.FirstName,
        LastName = billingDetails.LastName,
        Address = billingDetails.Address.ToDto()
    };

}
