using YourBrand.Orders.Domain.Entities;

namespace YourBrand.Orders.Domain.Events;

public sealed record OrderStatusUpdated(string OrderId, OrderStatus NewStatus, OrderStatus OldStatus) : DomainEvent;