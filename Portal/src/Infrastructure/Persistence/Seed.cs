using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Portal.Domain.Entities;

namespace YourBrand.Portal.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(DbContext context)
    {
        await context.SaveChangesAsync();
    }
}