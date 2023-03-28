using YourBrand.Portal.AppBar;
using YourBrand.Portal.Authentication;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace YourBrand.Portal.Authentication;

public static class ServicesProvider
{
    public static IServiceProvider UseAuthentication(this IServiceProvider services) 
    {
        var appBarTray = services
            .GetRequiredService<IAppBarTrayService>();

        var t = services.GetRequiredService<IStringLocalizer<AppBar.AppBar>>();

        var loginDisplayId = "Shell.LoginDisplay";

        appBarTray.AddItem(new AppBarTrayItem(loginDisplayId, () => t["Login"], typeof(LoginDisplay)));
        
        return services;
    }
}