using Microsoft.AspNetCore.Builder;
using YourBrand.Customers.Application.Hubs;

namespace YourBrand.Customers.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TodosHub>("/hubs/todos");

        return app;
    }
}
