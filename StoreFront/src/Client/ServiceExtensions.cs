using Microsoft.Extensions.DependencyInjection;

using YourBrand.StoreFront.Client;

namespace YourBrand.StoreFront.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddStoreFrontClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddAnalyticsClient(configureClient, builder)
            .AddCartClient(configureClient, builder)
            .AddCheckoutClient(configureClient, builder)
            .AddItemsClient(configureClient, builder)
            .AddUserClient(configureClient, builder);

        return services;
    }

    public static IServiceCollection AddAnalyticsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(AnalyticsClient) + "SF", configureClient)
            .AddTypedClient<AnalyticsClient>((http, sp) => new AnalyticsClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddCartClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(CartClient) + "SF", configureClient)
            .AddTypedClient<ICartClient>((http, sp) => new CartClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddCheckoutClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(CheckoutClient) + "SF", configureClient)
            .AddTypedClient<ICheckoutClient>((http, sp) => new CheckoutClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddItemsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(IItemsClient) + "SF", configureClient)
            .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddUserClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(UserClient) + "SF", configureClient)
            .AddTypedClient<IUserClient>((http, sp) => new UserClient(http));

        builder?.Invoke(b);

        return services;
    }
}