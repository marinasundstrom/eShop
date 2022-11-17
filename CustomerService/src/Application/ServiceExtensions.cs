using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using YourBrand.CustomerService.Application.Behaviors;

namespace YourBrand.CustomerService.Application;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceExtensions));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(typeof(ServiceExtensions).Assembly);

        return services;
    }
}
