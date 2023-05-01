using YourBrand.Portal.AppBar;
using YourBrand.Portal.NavMenu;
using YourBrand.Portal.Services;
using YourBrand.Portal.Theming;
using Microsoft.Extensions.DependencyInjection;
using YourBrand.Portal.Widgets;
using YourBrand.Portal.Markdown;

namespace YourBrand.Portal;

public static class ServiceExtensions 
{
    public static IServiceCollection AddShellServices(this IServiceCollection services) 
    {
        services
            .AddWidgets()
            .AddThemeServices()
            .AddNavigationServices()
            .AddAppBar()
            .AddScoped<CustomAuthorizationMessageHandler>()
            .AddScoped<ICurrentUserService, CurrentUserService>()
            .AddScoped<Services.IAccessTokenProvider, AccessTokenProvider>()
            .AddMarkdownServices();

        return services;
    }
}