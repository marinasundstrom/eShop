using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Sales.Domain.Entities;

namespace YourBrand.Sales.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(DbContext context)
    {
        await context.SaveChangesAsync();
    }
}