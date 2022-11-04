using YourBrand.Sales.Domain.Entities;

namespace YourBrand.Sales.Domain.Events;

public sealed record OrderStatusUpdated(string OrderId, OrderStatus NewStatus, OrderStatus OldStatus) : DomainEvent;