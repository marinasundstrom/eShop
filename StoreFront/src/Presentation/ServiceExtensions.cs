using Microsoft.Extensions.DependencyInjection;
using YourBrand.StoreFront.Application.Services;
using YourBrand.StoreFront.Presentation.Controllers;
using YourBrand.StoreFront.Presentation.Hubs;

namespace YourBrand.StoreFront.Presentation;

public static class ServiceExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        services.AddScoped<ICartHubService, CartHubService>();

        return services;
    }

    public static IServiceCollection AddControllersForApp(this IServiceCollection services)
    {
        var assembly = typeof(AnalyticsController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}
