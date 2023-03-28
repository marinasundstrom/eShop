using YourBrand.Portal.AppBar;
using YourBrand.Portal.Authentication;
using YourBrand.Portal.Localization;
using YourBrand.Portal.Theming;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace YourBrand.Portal;

public static class ShellInit
{
    public static IServiceProvider InitShell(this IServiceProvider services) 
    {
        var appBarTray = services
            .GetRequiredService<IAppBarTrayService>();

        var t = services.GetRequiredService<IStringLocalizer<AppBar.AppBar>>();
        
        var themeSelectorId = "Shell.ThemeSelector";

        appBarTray.AddItem(new AppBarTrayItem(themeSelectorId, t["theme"], typeof(ThemeSelector)));

        var loginDisplayId = "Shell.LoginDisplay";

        appBarTray.AddItem(new AppBarTrayItem(loginDisplayId, t["Login"], typeof(LoginDisplay)));

        var localeSelector = "Shell.LocaleSelector";

        appBarTray.AddItem(new AppBarTrayItem(localeSelector, t["ChangeLocale"], MudBlazor.Icons.Material.Filled.Language, async () => { 
            var dialogService = services.GetRequiredService<IDialogService>();
            var dialogRef = dialogService.Show<CultureSelector>(t["ChangeLocale"]);
            await dialogRef.Result;
        }));

        return services;
    }
}