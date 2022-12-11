using MediatR;

namespace YourBrand.Pricing.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}