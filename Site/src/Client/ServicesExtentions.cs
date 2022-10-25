using System.Globalization;
using Blazor.Analytics;

namespace Site.Client;

public static class ServiceExtensions 
{
    public static IServiceCollection AddServices(this IServiceCollection services) 
    {
        services.AddHttpClient("Site")
            .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http));

        services.AddHttpClient("Site")
            .AddTypedClient<ICartsClient>((http, sp) => new CartsClient(http));
         
        CultureInfo? culture = new("sv-SE");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        services.AddGoogleAnalytics("YOUR_GTAG_ID");

        return services;
    }
}