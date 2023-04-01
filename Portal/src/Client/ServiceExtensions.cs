using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Portal.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddWidgetsClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddWidgetsClient(configureClient, builder);

        return services;
    }

    public static IServiceCollection AddWidgetsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(WidgetsClient), configureClient)
            .AddTypedClient<IWidgetsClient>((http, sp) => new WidgetsClient(http));

        builder?.Invoke(b);

        return services;
    }
}