using Microsoft.AspNetCore.Builder;
using YourBrand.Catalog.Presentation.Hubs;

namespace YourBrand.Catalog.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
