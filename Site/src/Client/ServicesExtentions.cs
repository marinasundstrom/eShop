using System.Globalization;
using Blazor.Analytics;
using Blazored.LocalStorage;
using Blazored.Modal;
using Blazored.Toast;
using Site.Services;
namespace Site.Client;

public static class ServiceExtensions 
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) 
    {
        services.AddHttpClient("Site")
            .AddTypedClient<IItemsClient>((http, sp) => new ItemsClient(http));

        services.AddHttpClient("Site")
            .AddTypedClient<ICartClient>((http, sp) => new CartClient(http));

        services.AddHttpClient("Site")
            .AddTypedClient<ICheckoutClient>((http, sp) => new CheckoutClient(http));

        services.AddHttpClient("Site")
            .AddTypedClient<IUserClient>((http, sp) => new UserClient(http));

        services.AddHttpClient("Site")
            .AddTypedClient<IAnalyticsClient>((http, sp) => new AnalyticsClient(http));

        services.AddSiteServices();

        services.AddSingleton<RenderingContext>();

        services.AddScoped<Site.Services.IAccessTokenProvider, Site.Client.AccessTokenProvider>();
         
        CultureInfo? culture = new("sv-SE");
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        services.AddGoogleAnalytics(configuration["GoogleAnalytics:TrackingId"]);

        services.AddBlazoredLocalStorage();

        services.AddBlazoredModal();
        services.AddBlazoredToast();

        return services;
    }
}