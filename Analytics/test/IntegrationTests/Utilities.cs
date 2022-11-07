using YourBrand.Analytics.Infrastructure.Persistence;

namespace YourBrand.Analytics.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}