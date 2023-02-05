using Microsoft.AspNetCore.Builder;

using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<CartHub>("/hubs/cart");

        return app;
    }
}
