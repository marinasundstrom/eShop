using Microsoft.AspNetCore.Builder;
using YourBrand.CustomerService.Presentation.Hubs;

namespace YourBrand.CustomerService.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TicketsHub>("/hubs/tickets");

        return app;
    }
}
