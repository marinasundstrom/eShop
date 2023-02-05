using System;

using YourBrand.Customers.Application.Features.Addresses;

namespace YourBrand.Customers.Application.Features.Organizations;

public record OrganizationDto(int Id, string Name, string OrgNo, bool HasCeased, string? Phone, string? PhoneMobile, string? Email, IEnumerable<AddressDto> Addresses);
