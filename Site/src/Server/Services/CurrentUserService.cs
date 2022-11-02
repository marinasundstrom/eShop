using System;

namespace Site.Server.Services;

public class CurrentUserService : ICurrentUserService
{
    private HttpContext httpContext;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext!;
    }

    public int CustomerNo => int.Parse(httpContext.User.Claims.First(x => x.Type == "CustomerId")?.Value!);
}

