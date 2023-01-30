namespace YourBrand.Catalog.Common.Models;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);