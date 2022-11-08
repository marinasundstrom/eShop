using YourBrand.StoreFront.Domain;

namespace YourBrand.StoreFront.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}