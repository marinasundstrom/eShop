namespace YourBrand.Subscriptions.Application.ProductPriceLists.Dtos;

using YourBrand.Subscriptions.Application.Users;

public sealed record ProductPriceListDto(string Id, IEnumerable<ProductPriceDto> ProductPrices, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);

public sealed record ProductPriceDto(string Id, string ProductId, CurrencyAmountDto Price, DateTimeOffset Created, UserDto? CreatedBy, DateTimeOffset? LastModified, UserDto? LastModifiedBy);
