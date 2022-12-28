using MediatR;

namespace YourBrand.Shops.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}