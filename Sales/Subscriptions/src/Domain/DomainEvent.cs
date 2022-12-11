using MediatR;

namespace YourBrand.Subscriptions.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}