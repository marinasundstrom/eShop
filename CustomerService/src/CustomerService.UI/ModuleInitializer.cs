using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.CustomerService;
using YourBrand.CustomerService.Client;
using YourBrand.Portal.Navigation;
using YourBrand.Portal.Modules;
using Microsoft.Extensions.Localization;
using YourBrand.Portal;

namespace YourBrand.CustomerService;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddCustomerServiceClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.CustomerServiceServiceUrl}/");
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

        var group = navManager.GetGroup("customer-relations") ?? navManager.CreateGroup("customer-relations", () => resources["Customer relations"]);
        group.RequiresAuthorization = true;

        var group2 = group.GetGroup("customer-support") ?? group.CreateGroup("customer-support", () => resources["Support"],  MudBlazor.Icons.Material.Filled.Support);

        group2.CreateItem("board", () => resources["Board"], MudBlazor.Icons.Material.Filled.TableView, "/Tickets/Board");

        group2.CreateItem("tickets", () => resources["Tickets"], MudBlazor.Icons.Material.Filled.InsertDriveFile, "/Tickets");
    }
}