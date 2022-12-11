using System;
namespace YourBrand.Customers;

public static class AddressFormatter
{
    public static string ToAddressString(this AddressDto address) => $"{address.Thoroughfare} {address.Premises} {address.SubPremises}, {address.PostalCode} {address.Locality} {address.Country}";
}
