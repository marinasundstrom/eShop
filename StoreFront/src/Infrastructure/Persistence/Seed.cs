using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.StoreFront.Domain.Entities;

namespace YourBrand.StoreFront.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        /*
        if (!context.Contacts.Any())
        {


            await context.SaveChangesAsync();
        }
        */
    }
}