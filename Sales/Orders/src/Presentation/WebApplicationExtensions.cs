using Microsoft.AspNetCore.Builder;
using YourBrand.Orders.Presentation.Hubs;

namespace YourBrand.Orders.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
