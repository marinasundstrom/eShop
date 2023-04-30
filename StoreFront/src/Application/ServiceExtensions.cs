using System.Text.Json;
using System.Text.Json.Serialization;

using FluentValidation;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using YourBrand.StoreFront.Application.Behaviors;
using YourBrand.StoreFront.Application.Features.Analytics;
using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceExtensions));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        services.AddScoped<IStoresProvider, StoresProvider>();
        services.AddScoped<IStoreHandleToStoreIdResolver, StoreHandleToStoreIdResolver>();

        return services;
    }

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
