using Microsoft.AspNetCore.Builder;
using YourBrand.Portal.Hubs;

namespace YourBrand.Portal;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
