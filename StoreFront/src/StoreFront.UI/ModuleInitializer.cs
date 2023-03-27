using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.StoreFront.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.Navigation;
using Microsoft.Extensions.Localization;

namespace YourBrand.StoreFront;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddStoreFrontClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.StoreFrontServiceUrl}/");
        }, builder =>
        {
            builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });
    }

    public static void ConfigureServices(IServiceProvider services)
    {
        var navManager = services
            .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<Resources>>();

        var group = navManager.GetGroup("marketing") ?? navManager.CreateGroup("marketing", options =>
        {
            options.SetName(() => t["StoreFront"]);
            options.RequiresAuthorization = true;
        });

        group.CreateItem("analytics", () => t["StoreFront"], MudBlazor.Icons.Material.Filled.Analytics, "/analytics");
    }
}