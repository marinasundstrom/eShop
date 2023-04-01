using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YourBrand.Portal.Domain.Entities;

namespace YourBrand.Portal.Infrastructure.Persistence;

public static class Seed
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        context.Set<Widget>().AddRange(
            new Widget("analytics.engagements", "dashboard",  null, null),
            new Widget("sample-widget", "dashboard", null, null));

        await context.SaveChangesAsync();
    }
}