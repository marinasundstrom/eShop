namespace YourBrand.Catalog.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
}