using Microsoft.AspNetCore.Builder;
using YourBrand.StoreFront.Presentation.Hubs;

namespace YourBrand.StoreFront.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
