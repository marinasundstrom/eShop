using Microsoft.AspNetCore.Builder;
using YourBrand.Inventory.Presentation.Hubs;

namespace YourBrand.Inventory.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
