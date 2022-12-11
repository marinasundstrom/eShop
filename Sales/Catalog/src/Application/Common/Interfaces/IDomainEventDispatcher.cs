using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}