using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Carts.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddCartsClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddCartsClient(configureClient, builder);

        return services;
    }
    public static IServiceCollection AddCartsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(CartsClient), configureClient)
            .AddTypedClient<ICartsClient>((http, sp) => new CartsClient(http));

        builder?.Invoke(b);

        return services;
    }
}