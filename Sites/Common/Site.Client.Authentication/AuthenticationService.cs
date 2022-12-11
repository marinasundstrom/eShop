using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Site.Client.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _client;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private JsonSerializerOptions? _options = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };

    public AuthenticationService(HttpClient client, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
    {
        this._client = client;
        this._authStateProvider = authenticationStateProvider;
        this._localStorage = localStorage;
    }

    public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
    {
        var content = JsonSerializer.Serialize(userForAuthentication);
        var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
        var authResult = await _client.PostAsync("https://joes.yourbrand.local:5151/authentication/login", bodyContent);

        if (authResult.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new AuthResponseDto { IsAuthSuccessful = false };
        }

        var authContent = await authResult.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<AuthResponseDto>(authContent, _options);

        if (!authResult.IsSuccessStatusCode)
            return result;

        await _localStorage.SetItemAsync("authToken", result.Token);
        await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

        ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(result.Token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);

        return new AuthResponseDto { IsAuthSuccessful = true };
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
        _client.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string> RefreshToken()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
        var tokenDto = JsonSerializer.Serialize(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
        var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");
        var refreshResult = await _client.PostAsync("https://joes.yourbrand.local:5151/token/refresh", bodyContent);
        var refreshContent = await refreshResult.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<AuthResponseDto>(refreshContent, _options);

        if (!refreshResult.IsSuccessStatusCode)
            throw new ApplicationException("Something went wrong during the refresh token action");

        await _localStorage.SetItemAsync("authToken", result.Token);
        await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
        return result.Token;
    }
}

public class MockAuthenticationService : IAuthenticationService
{
    public Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
    {
        throw new NotImplementedException();
    }

    public Task Logout()
    {
        throw new NotImplementedException();
    }

    public Task<string> RefreshToken()
    {
        throw new NotImplementedException();
    }
}