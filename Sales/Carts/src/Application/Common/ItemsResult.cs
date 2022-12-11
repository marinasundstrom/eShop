namespace YourBrand.Carts.Application.Common;

public sealed record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);