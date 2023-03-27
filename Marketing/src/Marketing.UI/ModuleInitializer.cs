using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Marketing.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.NavMenu;
using Microsoft.Extensions.Localization;

namespace YourBrand.Marketing;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddMarketingClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.MarketingServiceUrl}/");
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
            options.SetName(() => t["Marketing"]);
            options.RequiresAuthorization = true;
        });

        group.CreateItem("campaigns", () => t["Campaigns"], MudBlazor.Icons.Material.Filled.List, "/marketing/campaigns");

        group.CreateItem("discounts", () => t["Discounts"], MudBlazor.Icons.Material.Filled.Discount, "/marketing/discounts");

        /*

        group.CreateItem("contacts", () => t["Contacts"], MudBlazor.Icons.Material.Filled.Person, "/contacts");

        var group2 = group.CreateGroup("campaigns", () => t["Campaigns"], MudBlazor.Icons.Material.Filled.ListAlt);

        group2.CreateItem("list", () => t["List"], MudBlazor.Icons.Material.Filled.ListAlt, "/campaigns");

        */
    }
}