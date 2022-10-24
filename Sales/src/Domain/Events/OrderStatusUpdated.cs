using YourBrand.Sales.Domain.Enums;

namespace YourBrand.Sales.Domain.Events;

public sealed record OrderStatusUpdated(string OrderId, OrderStatus NewStatus, OrderStatus OldStatus) : DomainEvent;