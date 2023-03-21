using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Analytics.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.NavMenu;
using Microsoft.Extensions.Localization;

namespace YourBrand.Analytics;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddAnalyticsClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.AnalyticsServiceUrl}/");
        }, builder =>
        {
            builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });
    }

    public static void ConfigureServices(IServiceProvider services)
    {
        var navManager = services
            .GetRequiredService<NavManager>();

        var resources = services.GetRequiredService<IStringLocalizer<Resources>>();

        var group = navManager.GetGroup("analytics") ?? navManager.CreateGroup("analytics", options =>
        {
            options.SetName(() => resources["Analytics"]);
            options.RequiresAuthorization = true;
        });

        group.CreateItem("engagement", () => resources["Engagement"], MudBlazor.Icons.Material.Filled.Analytics, "/analytics/engagement");
    }
}