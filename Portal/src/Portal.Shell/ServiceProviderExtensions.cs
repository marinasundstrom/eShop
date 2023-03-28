using YourBrand.Portal.AppBar;
using YourBrand.Portal.Authentication;
using YourBrand.Portal.Localization;
using YourBrand.Portal.NavMenu;
using YourBrand.Portal.Theming;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace YourBrand.Portal;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseShell(this IServiceProvider services)
    {
        CreateNavMenu(services);

        return services
            .UseTheming()
            .UseLocalization()
            .UseAuthentication();
    }

    private static void CreateNavMenu(IServiceProvider services)
    {
        var navManager = services
            .GetRequiredService<NavManager>();

        var t = services.GetRequiredService<IStringLocalizer<YourBrand.Portal.Resources>>();

        navManager.CreateItem("home", options =>
        {
            options.NameFunc = () => t["Home"];
            options.Icon = MudBlazor.Icons.Material.Filled.Home;
            options.Href = "/";
            options.RequiresAuthorization = false;
            options.Index = 0;
        });

        /*
        var group = navManager.GetGroup("administration") ?? navManager.CreateGroup("administration", () => t["Administration"]);

        group.CreateItem("users", options =>
        {
            options.NameFunc = () => t["Users"];
            options.Icon = MudBlazor.Icons.Material.Filled.Person;
            options.Href = "/users";
            options.RequiresAuthorization = true;
        });

        group.CreateItem("setup", options =>
        {
            options.NameFunc = () => t["SetUp"];
            options.Icon = MudBlazor.Icons.Material.Filled.Settings;
            options.Href = "/setup";
        });
        */
    }
}