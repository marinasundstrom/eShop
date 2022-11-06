using Microsoft.Extensions.DependencyInjection;

namespace Site.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddSiteServices(this IServiceCollection services)
    {
        services.AddScoped<CartService>();
        services.AddScoped<CartHubClient>();

        services.AddScoped<AnalyticsService>();

        return services;
    }
}