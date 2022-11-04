using Microsoft.AspNetCore.Builder;
using YourBrand.Marketing.Presentation.Hubs;

namespace YourBrand.Marketing.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
