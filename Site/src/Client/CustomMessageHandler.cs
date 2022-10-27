using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Site.Client.Authentication;

namespace Site.Client;

public class CustomMessageHandler : System.Net.Http.DelegatingHandler
{
    private readonly RefreshTokenService _refreshTokenService;
    private readonly ILocalStorageService localStorageService;

    public CustomMessageHandler(RefreshTokenService refreshTokenService, ILocalStorageService localStorageService)
    {
        _refreshTokenService = refreshTokenService;
        this.localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var absPath = request.RequestUri.AbsolutePath;

        Console.WriteLine(absPath);

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
}