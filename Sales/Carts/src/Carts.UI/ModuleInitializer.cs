using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Carts.Client;
using YourBrand.Portal.Navigation;
using YourBrand.Portal.Modules;
using Microsoft.Extensions.Localization;

namespace YourBrand.Carts;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddCartsClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.CartsServiceUrl}/");
        }, builder =>
        {
            //builder.AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        });
    }

    public static void ConfigureServices(IServiceProvider services)
    {
        var navManager = services
            .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<Resources>>();

        var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => t["Sales"]);
        group.RequiresAuthorization = true;

        group.CreateItem("carts", () => t["Carts"], MudBlazor.Icons.Material.Filled.InsertDriveFile, "/carts");
    }
}