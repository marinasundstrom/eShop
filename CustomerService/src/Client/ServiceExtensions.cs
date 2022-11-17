using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.CustomerService.Client;

public static class ServiceExtensions
{
    public static IServiceCollection AddCustomerServiceClients(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        services
            .AddCustomerServiceClient(configureClient, builder);

        return services;
    }
    public static IServiceCollection AddCustomerServiceClient(this IServiceCollection services, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? builder = null)
    {
        var b = services
            .AddHttpClient(nameof(CustomerServiceClient), configureClient)
            .AddTypedClient<ICustomerServiceClient>((http, sp) => new CustomerServiceClient(http));

        builder?.Invoke(b);

        return services;
    }
}