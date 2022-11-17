using Microsoft.Extensions.DependencyInjection;
using YourBrand.CustomerService.Application.Services;
using YourBrand.CustomerService.Presentation.Controllers;
using YourBrand.CustomerService.Presentation.Hubs;

namespace YourBrand.CustomerService.Presentation;

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
