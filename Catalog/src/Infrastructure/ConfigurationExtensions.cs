﻿using Microsoft.Extensions.Configuration;
using YourBrand.Catalog.Infrastructure;

namespace YourBrand.Catalog.Infrastructure;

public static class ConfigurationExtensions
{
    public static string? GetConnectionString(this IConfiguration configuration, string name, string database)
    {
        var connectionString = configuration.GetConnectionString(name);
        if (connectionString is null)
        {
            return null;
        }
        return $"{connectionString};Database={database}";
    }
}