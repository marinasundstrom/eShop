namespace Catalog.Application.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
}
