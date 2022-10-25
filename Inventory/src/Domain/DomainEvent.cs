using MediatR;

namespace YourBrand.Inventory.Domain
{
    public abstract record DomainEvent : INotification
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}