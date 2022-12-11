using Microsoft.AspNetCore.Builder;
using YourBrand.Ticketing.Presentation.Hubs;

namespace YourBrand.Ticketing.Presentation;

public static class WebApplicationExtensions
{
    public static WebApplication MapHubsForApp(this WebApplication app)
    {
        app.MapHub<TicketsHub>("/hubs/tickets");

        return app;
    }
}
