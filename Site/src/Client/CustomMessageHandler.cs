using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Site.Client.Authentication;

namespace Site.Client;

public class CustomMessageHandler : System.Net.Http.DelegatingHandler
{
    private readonly RefreshTokenService _refreshTokenService;
    private readonly ILocalStorageService localStorageService;
    private readonly NavigationManager navigationManager;

    public CustomMessageHandler(RefreshTokenService refreshTokenService, ILocalStorageService localStorageService, NavigationManager navigationManager)
    {
        _refreshTokenService = refreshTokenService;
        this.localStorageService = localStorageService;
        this.navigationManager = navigationManager;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var absPath = request.RequestUri.AbsolutePath;

        Console.WriteLine(absPath);

        try 
        {

            if (!absPath.Contains("Token") && !absPath.Contains("Authentication"))
            {
                var token = await _refreshTokenService.TryRefreshToken();
                
                if (string.IsNullOrEmpty(token))
                {
                    token = await localStorageService.GetItemAsync<string>("authToken");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

            }

            return await base.SendAsync(request, cancellationToken);
        }
        catch(Exception) 
        {
            throw;
        }
    }
}