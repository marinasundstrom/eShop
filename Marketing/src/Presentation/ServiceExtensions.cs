using Microsoft.Extensions.DependencyInjection;
using YourBrand.Marketing.Application.Services;
using YourBrand.Marketing.Presentation.Controllers;
using YourBrand.Marketing.Presentation.Hubs;

namespace YourBrand.Marketing.Presentation;

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
        var assembly = typeof(ContactsController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
