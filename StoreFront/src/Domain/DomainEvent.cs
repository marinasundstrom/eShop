using MediatR;

namespace YourBrand.StoreFront.Domain
{
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}