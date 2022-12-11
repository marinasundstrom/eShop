using Microsoft.AspNetCore.Builder;
using YourBrand.Pricing.Presentation.Hubs;

namespace YourBrand.Pricing.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<OrdersHub>("/hubs/orders");

        return app;
    }
}
