using Microsoft.AspNetCore.Builder;
using YourBrand.Carts.Application.Hubs;

namespace YourBrand.Carts.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        //app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
