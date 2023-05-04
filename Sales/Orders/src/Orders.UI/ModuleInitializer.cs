using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

using YourBrand.Portal;
using YourBrand.Orders.Client;
using YourBrand.Portal.NavMenu;
using YourBrand.Portal.Modules;
using Microsoft.Extensions.Localization;
using YourBrand.Portal.Widgets;
using YourBrand.Sales;

namespace YourBrand.Orders;

public class ModuleInitializer : IModuleInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddScoped<CustomAuthorizationMessageHandler>();

        services.AddOrdersClients((sp, httpClient) =>
        {
            var navigationManager = sp.GetRequiredService<NavigationManager>();
            httpClient.BaseAddress = new Uri($"{ServiceUrls.SalesServiceUrl}/");
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

        var group = navManager.GetGroup("sales") ?? navManager.CreateGroup("sales", () => t["Orders"]);
        group.RequiresAuthorization = true;

        group.CreateItem("orders", () => t["Orders"], MudBlazor.Icons.Material.Filled.InsertDriveFile, "/orders");

        var widgetService =
            services.GetRequiredService<IWidgetService>();

        widgetService.RegisterWidget(new Widget("orders.pendingOrders", "Pending orders", typeof(PendingOrdersWidget))
        {
            Size = WidgetSize.Medium
        });
    }
}