namespace YourBrand.Orders.Domain.Events;

public sealed record OrderDeleted(int OrderNo) : DomainEvent;