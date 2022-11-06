using Site.Client;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Blazor;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;

namespace Site.Services;

public sealed class AnalyticsService 
{
    private readonly IAnalyticsClient analyticsClient;
    private readonly ILocalStorageService localStorageService;
    private readonly ISessionStorageService sessionStorageService;
    private readonly IServiceProvider serviceProvider;
    string? cid;
    string? sid;

    public AnalyticsService(IAnalyticsClient analyticsClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService, IServiceProvider serviceProvider) 
    {
        this.analyticsClient = analyticsClient;
        this.localStorageService = localStorageService;
        this.sessionStorageService = sessionStorageService;
        this.serviceProvider = serviceProvider;
    }

    public async Task Init()
    {
        cid = await localStorageService.GetItemAsync<string?>("cid");

        if(cid is null) 
        {
            cid = await analyticsClient.CreateClientAsync();
            await localStorageService.SetItemAsync("cid", cid);
        }
          
        sid = await sessionStorageService.GetItemAsync<string?>("sid");

        if(sid is null) 
        {
            try 
            {
                sid = await analyticsClient.StartSessionAsync(cid);
                await sessionStorageService.SetItemAsync("sid", sid);
            }
            catch(Exception) 
            {
                await localStorageService.RemoveItemAsync("cid");

                await Init();
            }

            using var scope = serviceProvider.CreateScope();

            var geolocationService = scope.ServiceProvider.GetRequiredService<Microsoft.JSInterop.IGeolocationService>();

            geolocationService.GetCurrentPosition((args) => {
                analyticsClient.RegisterCoordinatesAsync(cid, sid, new Coordinates() { 
                    Latitude = (float)args.Coords.Latitude, 
                    Longitude = (float)args.Coords.Longitude  
                });
                
            }, (error) => {});
        }
    }

    public async Task RegisterEvent(EventData eventData) 
    {
        await analyticsClient.RegisterEventAsync(cid, sid, eventData);
    }
}