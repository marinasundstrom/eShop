using YourBrand.StoreFront.Infrastructure.Persistence;

namespace YourBrand.StoreFront.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}