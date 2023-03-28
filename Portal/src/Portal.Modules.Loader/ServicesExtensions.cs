using YourBrand.Portal.Modules;
using System.Net.Http.Json;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace YourBrand.Portal.Modules;

public static class ServicesExtensions
{
    public static IServiceCollection AddModuleLoader(this IServiceCollection services)
    {
        services.AddScoped<ModuleLoader>();

        return services;
    }

    public static async Task LoadModules(this IServiceCollection services, string path)
    {
        var http = services
            .BuildServiceProvider()
            .GetRequiredService<HttpClient>();

        var entries = await http.GetFromJsonAsync<IEnumerable<ModuleDefinition>>(path);

        entries!.Where(x => x.Enabled).ToList().ForEach(x =>
            ModuleLoader.LoadModule(x.Name, Assembly.Load(x.Assembly), x.Enabled));

        ModuleLoader.AddServices(services);
    }

    record ModuleEntry(Assembly Assembly, bool Enabled);

    public record ModuleDefinition(string Name, string Assembly, bool Enabled);
}