namespace YourBrand.Portal.Application.Common;

public sealed record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);