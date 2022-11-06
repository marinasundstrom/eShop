using System;

namespace Site.Server.Services;

public class CurrentUserService : ICurrentUserService
{
    private HttpContext httpContext;

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
}

