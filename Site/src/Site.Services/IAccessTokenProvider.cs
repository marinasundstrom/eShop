namespace Site.Services;

public interface IAccessTokenProvider 
{
    Task<string?> GetAccessToken();
}

