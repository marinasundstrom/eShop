using YourBrand.Inventory.Domain;

namespace YourBrand.Inventory.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}