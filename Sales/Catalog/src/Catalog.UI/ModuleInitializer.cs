using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Catalog.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.Navigation;
using Microsoft.Extensions.Localization;

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
    }

    public static void ConfigureServices(IServiceProvider services)
    {
        var navManager = services
            .GetRequiredService<NavManager>();

        var resources = services.GetRequiredService<IStringLocalizer<Resources>>();

        var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => resources["Sales"]);
        group.RequiresAuthorization = true;

        var catalogItem = group.CreateGroup("catalog", options =>
        {
            options.Name = resources["Catalog"];
            options.Icon = MudBlazor.Icons.Material.Filled.Book;
        });

        catalogItem.CreateItem("products", () => resources["Products"], MudBlazor.Icons.Material.Filled.FormatListBulleted, "/products");

        catalogItem.CreateItem("groups", () => resources["Groups"], MudBlazor.Icons.Material.Filled.Collections, "/products/groups");

        catalogItem.CreateItem("attributes", () => resources["Attributes"], MudBlazor.Icons.Material.Filled.List, "/products/attributes");

        catalogItem.CreateItem("brands", () => resources["Brands"], MudBlazor.Icons.Material.Filled.List, "/brands");
    }
}