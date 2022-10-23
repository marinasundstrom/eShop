using Microsoft.Extensions.DependencyInjection;
using YourBrand.Orders.Application.Services;
using YourBrand.Orders.Presentation.Controllers;
using YourBrand.Orders.Presentation.Hubs;

namespace YourBrand.Orders.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        services.AddScoped<IOrderNotificationService, OrderNotificationService>();

        return services;
    }

    public static IServiceCollection AddControllersForApp(this IServiceCollection services)
    {
        var assembly = typeof(OrdersController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
