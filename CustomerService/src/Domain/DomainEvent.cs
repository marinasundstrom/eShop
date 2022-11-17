using MediatR;

namespace YourBrand.CustomerService.Domain;

public abstract record DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}