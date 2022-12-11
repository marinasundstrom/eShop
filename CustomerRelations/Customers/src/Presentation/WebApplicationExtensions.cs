using Microsoft.AspNetCore.Builder;
using YourBrand.Customers.Presentation.Hubs;

namespace YourBrand.Customers.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
