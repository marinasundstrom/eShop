using System.Globalization;

using YourBrand.Portal.Services;
using YourBrand.Portal;

using Microsoft.JSInterop;

using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;
using YourBrand.Portal.Theming;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace YourBrand.Portal;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMudServices();

        services.AddBlazoredLocalStorage();
        
        services.AddLocalization();

        services.AddClients();

        return services;
    }

    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddHttpClient("WebAPI",
        client => client.BaseAddress = new Uri("https://localhost:5001/"));

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("WebAPI"));

        services.AddHttpClient<ITodosClient>(nameof(TodosClient), (sp, http) =>
        {
            http.BaseAddress = new Uri("https://localhost:5001/");
        })
        .AddTypedClient<ITodosClient>((http, sp) => new TodosClient(http))
        .AddHttpMessageHandler<CustomAuthorizationMessageHandler>()
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
        .AddPolicyHandler(GetRetryPolicy());

        services.AddHttpClient<IUsersClient>(nameof(UsersClient), (sp, http) =>
        {
            http.BaseAddress = new Uri("https://localhost:5001/");
        })
        .AddTypedClient<IUsersClient>((http, sp) => new UsersClient(http))
        .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
        //.SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
        //.AddPolicyHandler(GetRetryPolicy());


        return services;
    }


    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
             .HandleTransientHttpError()
             .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(1), retryCount: 5));
    }

    public static async Task Localize(this IServiceProvider serviceProvider)
    {
        CultureInfo culture;
        var js = serviceProvider.GetRequiredService<IJSRuntime>();
        var result = await js.InvokeAsync<string>("blazorCulture.get");

        if (result != null)
        {
            culture = new CultureInfo(result);
        }
        else
        {
            culture = new CultureInfo("en-US");
            await js.InvokeVoidAsync("blazorCulture.set", "en-US");
        }

        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
}