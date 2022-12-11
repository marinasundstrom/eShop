using Microsoft.Extensions.DependencyInjection;
using YourBrand.Ticketing.Application.Services;
using YourBrand.Ticketing.Presentation.Controllers;
using YourBrand.Ticketing.Presentation.Hubs;

namespace YourBrand.Ticketing.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        services.AddScoped<ITicketNotificationService, TicketNotificationService>();

        return services;
    }

    public static IServiceCollection AddControllersForApp(this IServiceCollection services)
    {
        var assembly = typeof(TicketsController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
