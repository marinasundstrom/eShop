namespace Catalog.Application.Services;

public interface IDateTime
{
    DateTimeOffset Now { get; }
}

