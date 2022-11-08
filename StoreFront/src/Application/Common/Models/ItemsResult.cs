using System;

namespace YourBrand.StoreFront.Application.Common.Models;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);