using MassTransit;
using YourBrand.CustomerService.Application;
using YourBrand.CustomerService.Consumers;
using YourBrand.CustomerService.Infrastructure;
using YourBrand.CustomerService.Presentation;

namespace YourBrand.CustomerService.Web;

public static class ServiceExtensions
{
    public static IServiceCollection AddUniverse(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPresentation()
            .AddApplication()
            .AddInfrastructure(configuration);

        return services;
    }

    public static IServiceCollection AddMassTransitForApp(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            //x.AddConsumers(typeof(Program).Assembly);

            x.AddConsumersForApp();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
