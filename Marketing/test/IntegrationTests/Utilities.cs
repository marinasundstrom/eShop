using YourBrand.Marketing.Infrastructure.Persistence;

namespace YourBrand.Marketing.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}