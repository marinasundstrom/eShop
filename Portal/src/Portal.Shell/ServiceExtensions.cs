using YourBrand.Portal.AppBar;
using YourBrand.Portal.Authentication;
using YourBrand.Portal.Localization;
using YourBrand.Portal.NavMenu;
using YourBrand.Portal.Services;
using YourBrand.Portal.Theming;
using MudBlazor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;


namespace YourBrand.Portal;

public static class ServiceExtensions 
{
    public static IServiceCollection AddShellServices(this IServiceCollection services) 
    {
        return services
            .AddThemeServices()
            .AddNavigationServices()
            .AddAppBar()
            .AddScoped<CustomAuthorizationMessageHandler>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<Services.IAccessTokenProvider, AccessTokenProvider>();
    }
}