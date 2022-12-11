using Microsoft.Extensions.DependencyInjection;
using YourBrand.Pricing.Application.Services;
using YourBrand.Pricing.Presentation.Controllers;
using YourBrand.Pricing.Presentation.Hubs;

namespace YourBrand.Pricing.Presentation;

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
