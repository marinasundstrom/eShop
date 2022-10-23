using MediatR;

namespace YourBrand.Orders.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}