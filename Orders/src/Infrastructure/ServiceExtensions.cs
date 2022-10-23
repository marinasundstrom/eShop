using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using YourBrand.Orders.Application.Common;
using YourBrand.Orders.Infrastructure.BackgroundJobs;
using YourBrand.Orders.Infrastructure.Idempotence;
using YourBrand.Orders.Infrastructure.Persistence;
using YourBrand.Orders.Infrastructure.Services;

namespace YourBrand.Orders.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IEmailService, EmailService>();

        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger => trigger.ForJob(jobKey)
                    .WithSimpleSchedule(schedule => schedule
                        .WithIntervalInSeconds(10)
                        .RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();

        return services;
    }
}
