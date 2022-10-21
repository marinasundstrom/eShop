using Catalog.Infrastructure.Persistence;

namespace Catalog.IntegrationTests;

internal class Utilities
{
    public static Task InitializeDbForTests(ApplicationDbContext db)
    {
        return Task.CompletedTask;
    }
}