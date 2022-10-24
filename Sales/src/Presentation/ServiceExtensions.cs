using Microsoft.Extensions.DependencyInjection;
using YourBrand.Sales.Application.Services;
using YourBrand.Sales.Presentation.Controllers;
using YourBrand.Sales.Presentation.Hubs;

namespace YourBrand.Sales.Presentation;

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
