﻿using MassTransit;
using YourBrand.Catalog.Application;
using YourBrand.Catalog.Consumers;
using YourBrand.Catalog.Infrastructure;
using YourBrand.Catalog.Presentation;

namespace YourBrand.Catalog.Web;

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
