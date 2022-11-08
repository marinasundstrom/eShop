using System;

namespace Site.Server.Services;

public class CurrentUserService : ICurrentUserService
{
    private HttpContext httpContext;
    string host;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext!;
    }

    public int? CustomerNo 
    {
        get 
        {
            var str = httpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "CustomerId")?.Value;
            
            if(str is null) return null;

            return int.Parse(str);
        }
    }

    public string? ClientId =>  httpContext?.Request.Headers["X-Client-Id"];

    public string? SessionId =>  httpContext?.Request.Headers["X-Session-Id"];

    public string? Host 
    {
        get 
        {
            var parts = httpContext?.Request.Host.Host.Split('.');
            if(parts!.Count() > 2) 
            {
                return host ??= parts!.First(); 
            }
            return null;
        }
    }
}

