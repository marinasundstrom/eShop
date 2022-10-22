using Microsoft.Extensions.DependencyInjection;
using YourBrand.Portal.Application.Services;
using YourBrand.Portal.Presentation.Controllers;
using YourBrand.Portal.Presentation.Hubs;

namespace YourBrand.Portal.Presentation;

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
        var assembly = typeof(TodosController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
