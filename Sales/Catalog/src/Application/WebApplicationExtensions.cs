using Microsoft.AspNetCore.Builder;

using YourBrand.Catalog.Hubs;

namespace YourBrand.Catalog;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}