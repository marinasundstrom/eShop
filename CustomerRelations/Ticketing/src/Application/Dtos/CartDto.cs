namespace YourBrand.Ticketing.Application.Ticketing.Dtos;

using YourBrand.Ticketing.Application.Users;

public sealed record IssueDto(string Id, IEnumerable<IssueItemDto> Items, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record IssueItemDto(string Id, string? ItemId, double Quantity, string? Data, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
