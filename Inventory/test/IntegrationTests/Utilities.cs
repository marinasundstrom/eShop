using YourBrand.Inventory.Infrastructure.Persistence;

namespace YourBrand.Inventory.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}