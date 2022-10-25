using Microsoft.Extensions.DependencyInjection;
using YourBrand.Inventory.Application.Services;
using YourBrand.Inventory.Presentation.Controllers;
using YourBrand.Inventory.Presentation.Hubs;

namespace YourBrand.Inventory.Presentation;

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
