using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Site.Client;
using Site.Client.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("Site", (sp, http) =>
{
    http.BaseAddress = new Uri(builder.Configuration["StorefrontUri"]);
    //http.EnableIntercept(sp);
}).AddHttpMessageHandler<CustomMessageHandler>();

builder.Services.AddScoped<CustomMessageHandler>();

builder.Services.AddAuthServices();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddAuthorizationCore();

var app = builder.Build();

var analyticsService = app.Services.GetRequiredService<Site.Services.AnalyticsService>();

await analyticsService.Init();

await app.RunAsync();
