using Microsoft.AspNetCore.Builder;
using YourBrand.Payments.Application.Features.Receipts;

namespace YourBrand.Payments.Application;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<ReceiptsHub>("/hubs/orders");

        return app;
    }
}
