using YourBrand.Pricing.Domain.Entities;

namespace YourBrand.Pricing.Domain.Events;

public sealed record OrderStatusUpdated(string OrderId, OrderStatus NewStatus, OrderStatus OldStatus) : DomainEvent;