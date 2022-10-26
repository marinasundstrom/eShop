using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Site.Client;
using Site.Client.Authentication;
using Toolbelt.Blazor.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("Site", (sp, http) => {
    http.BaseAddress =  new Uri(builder.HostEnvironment.BaseAddress);
    http.EnableIntercept(sp);
});

builder.Services.AddAuthServices();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddHttpClientInterceptor();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
