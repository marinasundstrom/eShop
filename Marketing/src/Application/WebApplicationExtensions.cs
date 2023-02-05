using Microsoft.AspNetCore.Builder;
using YourBrand.Marketing.Application.Hubs;

namespace YourBrand.Marketing.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
