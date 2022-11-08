namespace Site.Server.Services;

public interface ICurrentUserService
{
    int? CustomerNo { get; }

    string? ClientId { get; }

    string? SessionId { get; }

    string? Host { get; }
}