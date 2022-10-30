using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Catalog.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddCatalogClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddCatalogClient(configureClient, builder)
            .AddOptionsClient(configureClient, builder)
            .AddAttributesClient(configureClient, builder);

        return services;
    }

    public static IServiceCollection AddCatalogClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(ItemsClient), configureClient)
            .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http));

        builder?.Invoke(b);

        var b2 = services
            .AddHttpClient(nameof(ItemGroupsClient), configureClient)
            .AddTypedClient<IItemGroupsClient>((http, sp) => new ItemGroupsClient(http));

        builder?.Invoke(b2);

        return services;
    }

    public static IServiceCollection AddOptionsClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(OptionsClient), configureClient)
            .AddTypedClient<IOptionsClient>((http, sp) => new OptionsClient(http));

        builder?.Invoke(b);

        return services;
    }

    public static IServiceCollection AddAttributesClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(AttributesClient), configureClient)
            .AddTypedClient<IAttributesClient>((http, sp) => new AttributesClient(http));

        builder?.Invoke(b);

        return services;
    }
}