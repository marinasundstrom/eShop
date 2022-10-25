using YourBrand.Customers.Infrastructure.Persistence;

namespace YourBrand.Customers.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}