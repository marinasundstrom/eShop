using MediatR;

namespace YourBrand.Ticketing.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}