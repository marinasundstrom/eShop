using YourBrand.Portal.AppBar;
using YourBrand.Portal.Authentication;
using YourBrand.Portal.Theming;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Portal;

public static class ShellInit
{
    public static IServiceProvider InitShell(this IServiceProvider services) 
    {
        var appBarTray = services
            .GetRequiredService<IAppBarTrayService>();
        

        var themeSelectorId = "Shell.ThemeSelector";

        appBarTray.AddItem(new AppBarTrayItem(themeSelectorId, typeof(ThemeSelector)));

        var loginDisplayId = "Shell.LoginDisplay";

        appBarTray.AddItem(new AppBarTrayItem(loginDisplayId, typeof(LoginDisplay)));

        /*
        var itemId = "Shell.ThemeSelector";

        appBarTray.AddItem(new AppBarTrayItem(itemId, "Test", MudBlazor.Icons.Material.Filled.List, () => { 
            appBarTray.RemoveItem(itemId);
        }));
        */

        return services;
    }
}
