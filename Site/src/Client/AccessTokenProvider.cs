using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Blazored.LocalStorage;

namespace Site.Client;

public sealed class AccessTokenProvider : Site.Services.IAccessTokenProvider
{
    private readonly ILocalStorageService localStorageService;

    public AccessTokenProvider(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    public async Task<string> GetAccessToken() => await localStorageService.GetItemAsync<string>("authToken");
}