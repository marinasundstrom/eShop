using Blazored.LocalStorage;

using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Portal.AppBar;

public static class ServicesExtensions
{
    public static IServiceCollection AddAppBar(this IServiceCollection services)
    {
        services.AddScoped<IAppBarTrayService, AppBarTrayService>();
        
        return services;
    }
}