using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
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

        services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Default30",
                    new CacheProfile()
                    {
                        Duration = 30,
                        VaryByQueryKeys = new[] { "*" }
                    });
            })
            .AddApplicationPart(assembly)
            .AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });

        return services;
    }
}
