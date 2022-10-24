using Microsoft.Extensions.DependencyInjection;
using YourBrand.Catalog.Application.Services;
using YourBrand.Catalog.Presentation.Controllers;
using YourBrand.Catalog.Presentation.Hubs;

namespace YourBrand.Catalog.Presentation;

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
        var assembly = typeof(ItemsController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
