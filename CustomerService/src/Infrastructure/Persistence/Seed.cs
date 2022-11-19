using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.CustomerService.Domain.Entities;

namespace YourBrand.CustomerService.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        //context.Tickets.Add(new Issue("test"));

        await context.SaveChangesAsync();
    }
}