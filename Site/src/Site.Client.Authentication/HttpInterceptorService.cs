using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace Site.Client.Authentication;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly RefreshTokenService _refreshTokenService;

    public HttpInterceptorService(HttpClientInterceptor interceptor, RefreshTokenService refreshTokenService)
    {
        _interceptor = interceptor;
        _refreshTokenService = refreshTokenService;
    }

    public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

    public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request.RequestUri.AbsolutePath;

        if (!absPath.Contains("Token") && !absPath.Contains("Authentication"))
        {
            var token = await _refreshTokenService.TryRefreshToken();
            if (!string.IsNullOrEmpty(token))
            {
                e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
            }
        }
    }

    public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
}