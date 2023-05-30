using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Payments.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddReceiptsClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddReceiptsClient(configureClient, builder)
            .AddReceiptStatusesClient(configureClient, builder)
            .AddCartsClient(configureClient, builder);

        return services;
    }

    public static IServiceCollection AddReceiptsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(ReceiptsClient), configureClient)
            .AddTypedClient<IReceiptsClient>((http, sp) => new ReceiptsClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddReceiptStatusesClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(ReceiptStatusesClient), configureClient)
            .AddTypedClient<IReceiptStatusesClient>((http, sp) => new ReceiptStatusesClient(http));

        builder?.Invoke(b);

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