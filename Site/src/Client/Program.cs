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

/*
builder.Services.AddScoped<IAuthorizationPolicyProvider, Microsoft.AspNetCore.Authorization.DefaultAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthorizationService, Microsoft.AspNetCore.Authorization.DefaultAuthorizationService>();
builder.Services.AddScoped<IAuthorizationHandlerProvider, Microsoft.AspNetCore.Authorization.DefaultAuthorizationHandlerProvider>();
builder.Services.AddScoped<IAuthorizationHandlerContextFactory, Microsoft.AspNetCore.Authorization.DefaultAuthorizationHandlerContextFactory>();
builder.Services.AddScoped<IAuthorizationEvaluator, Microsoft.AspNetCore.Authorization.DefaultAuthorizationEvaluator>(); */

await builder.Build().RunAsync();
