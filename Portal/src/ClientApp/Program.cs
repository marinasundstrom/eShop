using System.Net.Http.Json;
using System.Reflection;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using MudBlazor.Services;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using YourBrand.Portal;
using YourBrand.Portal.Modules;
using YourBrand.Portal.Navigation;
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
    .AddScoped<ModuleLoader>();

await LoadModules(builder.Services);

var app = builder.Build();

var moduleBuilder = app.Services.GetRequiredService<ModuleLoader>();
moduleBuilder.ConfigureServices();

var navManager = app.Services
    .GetRequiredService<NavManager>();

var resources = app.Services.GetRequiredService<IStringLocalizer<YourBrand.Portal.Resources>>();

navManager.CreateItem("home", options =>
{
    options.NameFunc = () => resources["Home"];
    options.Icon = MudBlazor.Icons.Material.Filled.Home;
    options.Href = "/";
    options.RequiresAuthorization = false;
    options.Index = 0;
});

/*
var group = navManager.GetGroup("administration") ?? navManager.CreateGroup("administration", () => resources["Administration"]);

group.CreateItem("users", options =>
{
    options.NameFunc = () => resources["Users"];
    options.Icon = MudBlazor.Icons.Material.Filled.Person;
    options.Href = "/users";
    options.RequiresAuthorization = true;
});

group.CreateItem("setup", options =>
{
    options.NameFunc = () => resources["SetUp"];
    options.Icon = MudBlazor.Icons.Material.Filled.Settings;
    options.Href = "/setup";
});
*/

await app.Services.Localize();

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