using System.Net.Http.Json;
using System.Reflection;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;

using YourBrand.Portal;
using YourBrand.Portal.Localization;
using YourBrand.Portal.Modules;

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
    .AddShellServices()
    .AddModuleLoader();

await builder.Services.LoadModules($"/modules.json");

var app = builder.Build();

await app.Services.ApplyLocalization();

app.Services
    .UseShell()
    .UseModules();

await app.RunAsync();