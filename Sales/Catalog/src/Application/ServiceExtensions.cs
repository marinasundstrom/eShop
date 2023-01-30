using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using YourBrand.Catalog.Behaviors;
using YourBrand.Catalog.Features.Products;
using YourBrand.Catalog.Features.Products.Variants;
using YourBrand.Catalog.Hubs;

namespace YourBrand.Catalog;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceExtensions));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        services.AddScoped<ProductsService>();

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllersForApp();

        services.AddScoped<ITodoNotificationService, TodoNotificationService>();

        return services;
    }

    public static IServiceCollection AddControllersForApp(this IServiceCollection services)
    {
        var assembly = typeof(ProductsController).Assembly;

        services.AddControllers()
            .AddApplicationPart(assembly);

        return services;
    }
}