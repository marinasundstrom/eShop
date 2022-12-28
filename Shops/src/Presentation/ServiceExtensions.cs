using Microsoft.Extensions.DependencyInjection;
using YourBrand.Shops.Application.Services;
using YourBrand.Shops.Presentation.Controllers;

namespace YourBrand.Shops.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        //services.AddScoped<IOrderNotificationService, OrderNotificationService>();

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
