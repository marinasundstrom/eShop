using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Inventory.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.NavMenu;
using Microsoft.Extensions.Localization;

namespace YourBrand.Inventory;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddInventoryClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.InventoryServiceUrl}/");
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

        var group = navManager.GetGroup("inventory") ?? navManager.CreateGroup("inventory", () => t["Inventory"]);
        group.RequiresAuthorization = true;

        group.CreateItem("items", () => t["Items"], MudBlazor.Icons.Material.Filled.ListAlt, "/inventory/items");
        group.CreateItem("warehouses", () => t["Warehouses"], MudBlazor.Icons.Material.Filled.Warehouse, "/inventory/warehouses");
        //group.CreateItem("sites", () => t["Sites"], MudBlazor.Icons.Material.Filled.LocationCity, "/inventory/sites");
    }
}