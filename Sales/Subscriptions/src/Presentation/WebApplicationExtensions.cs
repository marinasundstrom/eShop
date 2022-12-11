using Microsoft.AspNetCore.Builder;
using YourBrand.Subscriptions.Presentation.Hubs;

namespace YourBrand.Subscriptions.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
