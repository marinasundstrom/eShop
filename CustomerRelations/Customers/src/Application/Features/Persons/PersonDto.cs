using System;

using YourBrand.Customers.Application.Features.Addresses;

namespace YourBrand.Customers.Application.Features.Persons;

public record PersonDto(int Id, string FirstName, string LastName, string SSN, bool IsDeceased, string? Phone, string? PhoneMobile, string? Email, IEnumerable<AddressDto> Addresses);
