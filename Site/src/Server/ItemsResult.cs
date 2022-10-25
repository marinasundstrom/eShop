using System;

namespace Site.Server;

public record ItemsResult<T>(IEnumerable<T> Items, int TotalItems);