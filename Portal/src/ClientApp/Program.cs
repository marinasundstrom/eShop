using System.Net.Http.Json;
using System.Reflection;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;

using YourBrand.Portal;
using YourBrand.Portal.AppBar;
using YourBrand.Portal.Modules;
using YourBrand.Portal.NavMenu;
using YourBrand.Portal.Theming;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Local", options.ProviderOptions);
});

builder.Services.AddScoped<YourBrand.Portal.Services.IAccessTokenProvider, YourBrand.Portal.Services.AccessTokenProvider>();

builder.Services
    .AddServices()
    .AddThemeServices()
    .AddNavigationServices()
    .AddAppBar()
    .AddScoped<ModuleLoader>();

await LoadModules(builder.Services);

var app = builder.Build();

await app.Services.Localize();
app.Services.InitShell();

var moduleBuilder = app.Services.GetRequiredService<ModuleLoader>();
moduleBuilder.ConfigureServices();

await app.RunAsync();

async Task LoadModules(IServiceCollection services)
{
    var http = builder.Services
        .BuildServiceProvider()
        .GetRequiredService<HttpClient>();

    var entries = await http.GetFromJsonAsync<IEnumerable<ModuleDefinition>>($"/modules.json");

    entries!.Where(x => x.Enabled).ToList().ForEach(x =>
        ModuleLoader.LoadModule(x.Name, Assembly.Load(x.Assembly), x.Enabled));

    ModuleLoader.AddServices(builder.Services);
}

record ModuleEntry(Assembly Assembly, bool Enabled);

public record ModuleDefinition(string Name, string Assembly, bool Enabled);