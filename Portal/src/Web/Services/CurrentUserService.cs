extern alias Application;

using System.Security.Claims;
using YourBrand.Portal.Services;

namespace YourBrand.Portal.Web.Services;

public sealed class CurrentUserService : Application::YourBrand.Portal.Services.ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? _currentUserId;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _currentUserId ??= _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}