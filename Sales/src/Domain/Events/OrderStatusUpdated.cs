using YourBrand.Sales.Domain.Entities;

namespace YourBrand.Sales.Domain.Events;

public sealed record OrderStatusUpdated(int OrderNo, OrderStatus NewStatus, OrderStatus OldStatus) : DomainEvent;