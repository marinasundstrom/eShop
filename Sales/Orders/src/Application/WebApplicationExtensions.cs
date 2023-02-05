using Microsoft.AspNetCore.Builder;
using YourBrand.Orders.Application.Features.Orders;

namespace YourBrand.Orders.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
