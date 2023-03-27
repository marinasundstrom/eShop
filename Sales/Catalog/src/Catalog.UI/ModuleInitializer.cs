using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Catalog.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.NavMenu;
using Microsoft.Extensions.Localization;
using YourBrand.Portal.AppBar;
using MudBlazor;

namespace YourBrand.Catalog;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddCatalogClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.CatalogServiceUrl}/");
        }, builder =>
        {
            //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });

        services.AddScoped<IStoreProvider, StoreProvider>();
    }

    public static void ConfigureServices(IServiceProvider services)
    {
        InitNavBar(services);
        InitAppBarTray(services);
    }

    private static void InitNavBar(IServiceProvider services) 
    {        
         var navManager = services
            .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<Resources>>();

        var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => t["Sales"]);
        group.RequiresAuthorization = true;

        var catalogItem = group.CreateGroup("catalog", options =>
        {
            options.Name = t["Catalog"];
            options.Icon = MudBlazor.Icons.Material.Filled.Book;
        });

        catalogItem.CreateItem("products", () => t["Products"], MudBlazor.Icons.Material.Filled.FormatListBulleted, "/products");

        catalogItem.CreateItem("groups", () => t["Groups"], MudBlazor.Icons.Material.Filled.Collections, "/products/groups");

        catalogItem.CreateItem("attributes", () => t["Attributes"], MudBlazor.Icons.Material.Filled.List, "/products/attributes");

        catalogItem.CreateItem("brands", () => t["Brands"], MudBlazor.Icons.Material.Filled.List, "/brands");
    }

    private static void InitAppBarTray(IServiceProvider services) 
    {
        var appBarTray = services
            .GetRequiredService<IAppBarTrayService>();

        var snackbar = services
            .GetRequiredService<ISnackbar>();

        appBarTray.AddItem(new AppBarTrayItem("show", typeof(StoreSelector)));

        appBarTray.AddItem(new AppBarTrayItem("show2", "Test", MudBlazor.Icons.Material.Filled.List, () => snackbar.Add ("Hello!") ));
    }
}