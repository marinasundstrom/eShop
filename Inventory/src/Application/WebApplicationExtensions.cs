using Microsoft.AspNetCore.Builder;
using YourBrand.Inventory.Application.Hubs;

namespace YourBrand.Inventory.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
