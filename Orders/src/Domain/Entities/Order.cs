using YourBrand.Orders.Domain.Enums;
using YourBrand.Orders.Domain.Events;

namespace YourBrand.Orders.Domain.Entities;

public class Order : AuditableEntity, IAggregateRoot
{
    protected Order()
    {
    }

    public Order(OrderStatus status = OrderStatus.NotStarted)
    {
        Status = status;
    }

    public int Id { get; private set; }


    public OrderStatus Status { get; private set; }

    public bool UpdateStatus(OrderStatus status)
    {
        var oldStatus = Status;
        if (status != oldStatus)
        {
            Status = status;

            AddDomainEvent(new OrderUpdated(Id));
            AddDomainEvent(new OrderStatusUpdated(Id, status, oldStatus));

            return true;
        }

        return false;
    }

    public User? AssignedTo { get; private set; }

    public string? AssignedToId { get; private set; }

    public bool UpdateAssignedTo(string? userId)
    {
        var oldAssignedToId = AssignedToId;
        if (userId != oldAssignedToId)
        {
            AssignedToId = userId;
            AddDomainEvent(new OrderAssignedUserUpdated(Id, userId, oldAssignedToId));

            return true;
        }

        return false;
    }
}
