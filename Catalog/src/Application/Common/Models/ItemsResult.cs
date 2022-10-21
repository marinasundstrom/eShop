using System;

namespace Catalog.Application.Common.Models;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);