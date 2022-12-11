using Microsoft.Extensions.DependencyInjection;
using YourBrand.Customers.Application.Services;
using YourBrand.Customers.Domain.Entities;
using YourBrand.Customers.Presentation.Controllers;
using YourBrand.Customers.Presentation.Hubs;

namespace YourBrand.Customers.Presentation;

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
        var assembly = typeof(Customer).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
