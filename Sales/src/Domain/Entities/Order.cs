using System.Collections.Generic;
using YourBrand.Sales.Domain.Enums;
using YourBrand.Sales.Domain.Events;

namespace YourBrand.Sales.Domain.Entities;

public class Order : AuditableEntity, IAggregateRoot
{
    HashSet<OrderItem> _items = new HashSet<OrderItem>();

    protected Order()
    {
    }

    public Order(OrderStatus status = OrderStatus.Draft)
    {
        Status = status;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public DateTime Date { get; private set; } = DateTime.Now;

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

    public User? Assignee { get; private set; }

    public string? AssigneeId { get; private set; }

    public bool UpdateAssigneeId(string? userId)
    {
        var oldAssigneeId = AssigneeId;
        if (userId != oldAssigneeId)
        {
            AssigneeId = userId;
            AddDomainEvent(new OrderAssignedUserUpdated(Id, userId, oldAssigneeId));

            return true;
        }

        return false;
    }

    public string? CustomerId { get; set; }

    public string Currency { get; set; } = "SEK";

    public double VatRate { get; set; } 

    public decimal Vat { get; set; }

    public decimal SubTotal { get; set; } 

    public decimal Total { get; set; } 

    public ValueObjects.BillingDetails? BillingDetails { get; set; } = null!;

    public ValueObjects.ShippingDetails? ShippingDetails { get; set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public OrderItem AddOrderItem(string description, string? itemId, string? unit, decimal unitPrice, double vatRate, double quantity) 
    {
        var orderItem = new OrderItem(description, itemId, unit, unitPrice, vatRate, quantity, unitPrice * (decimal)quantity);
        _items.Add(orderItem);
        return orderItem;
    }

    public void RemoveOrderItem(OrderItem orderItem) => _items.Remove(orderItem);

    public void Calculate()
    {
        VatRate = 0.25;
        Vat = Items.Sum(x => (decimal)x.VatRate * x.Total);
        Total = Items.Sum(x => x.Total);
        SubTotal = Total - Vat;
    }
}
