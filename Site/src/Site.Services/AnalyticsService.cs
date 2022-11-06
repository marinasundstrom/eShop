using Site.Client;
using Blazored.LocalStorage;
using Blazored.SessionStorage;

namespace Site.Services;

public sealed class AnalyticsService 
{
    private readonly IAnalyticsClient analyticsClient;
    private readonly ILocalStorageService localStorageService;
    private readonly ISessionStorageService sessionStorageService;
    string? cid;
    string? sid;

    public AnalyticsService(IAnalyticsClient analyticsClient, ILocalStorageService localStorageService, ISessionStorageService sessionStorageService) 
    {
        this.analyticsClient = analyticsClient;
        this.localStorageService = localStorageService;
        this.sessionStorageService = sessionStorageService;
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
            sid = await analyticsClient.StartSessionAsync(cid);
            await localStorageService.SetItemAsync("sid", sid);
        }
    }

    public async Task RegisterEvent(EventData eventData) 
    {
        await analyticsClient.RegisterEventAsync(cid, sid, eventData);
    }
}