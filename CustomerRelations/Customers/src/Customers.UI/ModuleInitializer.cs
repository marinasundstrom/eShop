using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Customers.Client;
using YourBrand.Portal.Modules;
using YourBrand.Portal.NavMenu;
using Microsoft.Extensions.Localization;

namespace YourBrand.Customers;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddCustomersClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.CustomersServiceUrl}/");
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

        var group = navManager.GetGroup("customer-relations") ?? navManager.CreateGroup("customer-relations", () => t["Customer relations"]);
        group.RequiresAuthorization = true;

        group.CreateItem("customers", () => t["Customers"], MudBlazor.Icons.Material.Filled.Person, "/customers");
    }
}