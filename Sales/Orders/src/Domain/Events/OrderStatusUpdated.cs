using YourBrand.Orders.Domain.Entities;

namespace YourBrand.Orders.Domain.Events;

public sealed record OrderStatusUpdated(string OrderId, int NewStatus, int OldStatus) : DomainEvent;