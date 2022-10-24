using Microsoft.AspNetCore.Builder;
using YourBrand.Sales.Presentation.Hubs;

namespace YourBrand.Sales.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
