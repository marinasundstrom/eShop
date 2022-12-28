using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Shops.Client;
using YourBrand.Portal.Navigation;
using YourBrand.Portal.Modules;
using Microsoft.Extensions.Localization;

namespace YourBrand.Shops;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddShopsClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.ShopServiceUrl}/");
        }, builder =>
        {
            //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });
    }

    public static void ConfigureServices(IServiceProvider services)
    {
        var navManager = services
            .GetRequiredService<NavManager>();

        var resources = services.GetRequiredService<IStringLocalizer<Resources>>();

        var group = navManager.GetGroup("settings") ?? navManager.CreateGroup("settings", () => resources["Settings"]);
        group.RequiresAuthorization = true;

        group.CreateItem("shops", () => resources["Shops"], MudBlazor.Icons.Material.Filled.Shop, "/shops");
    }
}