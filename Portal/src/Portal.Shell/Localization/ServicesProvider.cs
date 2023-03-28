using YourBrand.Portal.AppBar;
using YourBrand.Portal.Localization;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace YourBrand.Portal.Localization;

public static class ServicesProvider
{
    public static IServiceProvider UseLocalization(this IServiceProvider services) 
    {
        var appBarTray = services
            .GetRequiredService<IAppBarTrayService>();

        var t = services.GetRequiredService<IStringLocalizer<AppBar.AppBar>>();

        var localeSelector = "Shell.LocaleSelector";

        appBarTray.AddItem(new AppBarTrayItem(localeSelector, t["ChangeLocale"], MudBlazor.Icons.Material.Filled.Language, async () => { 
            var dialogService = services.GetRequiredService<IDialogService>();
            var dialogRef = dialogService.Show<CultureSelector>(t["ChangeLocale"]);
            await dialogRef.Result;
        }));

        return services;
    }
}