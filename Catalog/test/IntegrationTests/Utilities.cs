using YourBrand.Catalog.Infrastructure.Persistence;

namespace YourBrand.Catalog.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}