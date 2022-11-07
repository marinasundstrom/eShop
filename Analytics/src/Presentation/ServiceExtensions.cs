using Microsoft.Extensions.DependencyInjection;
using YourBrand.Analytics.Application.Services;
using YourBrand.Analytics.Presentation.Controllers;
using YourBrand.Analytics.Presentation.Hubs;

namespace YourBrand.Analytics.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        services.AddScoped<ITodoNotificationService, TodoNotificationService>();

        return services;
    }

    public static IServiceCollection AddControllersForApp(this IServiceCollection services)
    {
        var assembly = typeof(ClientController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
