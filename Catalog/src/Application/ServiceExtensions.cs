using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Catalog.Application.Behaviors;
using Catalog.Application.Products.Variants;

namespace Catalog.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceExtensions));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        services.AddScoped<ProductVariantsService>();

        return services;
    }
}
