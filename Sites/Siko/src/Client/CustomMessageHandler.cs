using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
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

        try
        {
            if (!absPath.Contains("Token") && !absPath.Contains("Authentication"))
            {
                string token;

                try
                {
                    token = await _refreshTokenService.TryRefreshToken();
                }
                catch (Exception exc)
                {
                    await localStorageService.RemoveItemAsync("authToken");
                    await localStorageService.RemoveItemAsync("refreshToken");

                    navigationManager.NavigateTo("/login");

                    return await base.SendAsync(request, cancellationToken);
                }

                if (string.IsNullOrEmpty(token))
                {
                    token = await localStorageService.GetItemAsync<string>("authToken");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
