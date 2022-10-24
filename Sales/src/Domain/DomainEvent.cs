using MediatR;

namespace YourBrand.Sales.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}