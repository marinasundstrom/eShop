using Microsoft.AspNetCore.Builder;
using YourBrand.Portal.Presentation.Hubs;

namespace YourBrand.Portal.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
