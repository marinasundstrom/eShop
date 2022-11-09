using Microsoft.AspNetCore.Builder;
using YourBrand.StoreFront.Presentation.Hubs;

namespace YourBrand.StoreFront.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<CartHub>("/hubs/cart");

        return app;
    }
}
