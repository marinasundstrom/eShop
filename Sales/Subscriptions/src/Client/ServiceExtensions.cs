using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Subscriptions.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddSalesClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        /*
        services
            .AddOrdersClient(configureClient, builder)
            .AddOrderStatusesClient(configureClient, builder)
            .AddCartsClient(configureClient, builder);
        */
        return services;
    }

    public static IServiceCollection AddOrdersClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(OrdersClient), configureClient)
            .AddTypedClient<IOrdersClient>((http, sp) => new OrdersClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddOrderStatusesClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(OrderStatusesClient), configureClient)
            .AddTypedClient<IOrderStatusesClient>((http, sp) => new OrderStatusesClient(http));

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