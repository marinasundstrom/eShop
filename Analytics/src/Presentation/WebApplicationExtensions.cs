using Microsoft.AspNetCore.Builder;
using YourBrand.Analytics.Presentation.Hubs;

namespace YourBrand.Analytics.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
