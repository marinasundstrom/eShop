using Microsoft.AspNetCore.Builder;

namespace YourBrand.Shops.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        //app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
