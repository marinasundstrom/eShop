using MediatR;

namespace YourBrand.Catalog.Common;

public interface IDomainEventHandler<TDomainEvent>
    : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{

}