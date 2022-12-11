using YourBrand.Subscriptions.Domain.Entities;

namespace YourBrand.Subscriptions.Domain.Events;

public sealed record OrderStatusUpdated(string OrderId, OrderStatus NewStatus, OrderStatus OldStatus) : DomainEvent;