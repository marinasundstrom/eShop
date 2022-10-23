using YourBrand.Orders.Domain.Enums;

namespace YourBrand.Orders.Domain.Events;

public sealed record OrderStatusUpdated(int OrderId, OrderStatus NewStatus, OrderStatus OldStatus) : DomainEvent;