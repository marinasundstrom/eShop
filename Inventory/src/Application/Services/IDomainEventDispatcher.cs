namespace YourBrand.Inventory.Application.Services;

public interface IDomainEventDispatcher
{
    Task Dispatch(DomainEvent domainEvent, CancellationToken cancellationToken = default);
}
