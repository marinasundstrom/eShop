using Microsoft.AspNetCore.Builder;
using YourBrand.Carts.Presentation.Hubs;

namespace YourBrand.Carts.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        //app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
