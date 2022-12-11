using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Carts.Domain.Entities;

namespace YourBrand.Carts.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        context.Carts.Add(new Cart("test"));

        await context.SaveChangesAsync();
    }
}