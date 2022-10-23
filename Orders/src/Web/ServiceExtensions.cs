using MassTransit;
using YourBrand.Orders.Application;
using YourBrand.Orders.Consumers;
using YourBrand.Orders.Infrastructure;
using YourBrand.Orders.Presentation;

namespace YourBrand.Orders.Web;

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
