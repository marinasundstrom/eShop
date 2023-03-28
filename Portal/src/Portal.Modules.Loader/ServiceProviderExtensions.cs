using System.Net.Http.Json;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Portal.Modules;

public static class ServiceProviderExtensions
{
    public static IServiceProvider UseModules(this IServiceProvider services)
    {
        var moduleBuilder = services.GetRequiredService<ModuleLoader>();
        moduleBuilder.ConfigureServices();

        return services;
    }
}