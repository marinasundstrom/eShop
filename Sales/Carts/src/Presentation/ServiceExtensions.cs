using Microsoft.Extensions.DependencyInjection;
using YourBrand.Carts.Application.Services;
using YourBrand.Carts.Presentation.Controllers;
using YourBrand.Carts.Presentation.Hubs;

namespace YourBrand.Carts.Presentation;

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
        var assembly = typeof(CartsController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
