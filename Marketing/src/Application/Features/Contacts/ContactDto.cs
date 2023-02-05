using System;

using YourBrand.Marketing.Application.Features.Campaigns;

namespace YourBrand.Marketing.Application.Features.Contacts;

public record ContactDto(string Id, CampaignDto? Campaign, string FirstName, string LastName, string SSN, string? Phone, string? PhoneMobile, string? Email, ContactAddressDto? Address);
