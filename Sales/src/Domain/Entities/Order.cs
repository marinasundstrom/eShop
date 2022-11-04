using System.Collections.Generic;
using YourBrand.Sales.Domain.Events;

namespace YourBrand.Sales.Domain.Entities;

public class Order : AggregateRoot<string>, IAuditable
{
    HashSet<OrderItem> _items = new HashSet<OrderItem>();

    public Order() : base(Guid.NewGuid().ToString())
    {
        StatusId = 1;
    }

    public int OrderNo { get; set; }

    public string CompanyId { get; private set; } = "ACME";

    public DateTime Date { get; private set; } = DateTime.Now;

    public OrderStatus Status { get; private set; } = null!;

    public int StatusId { get; set; } 

    public bool UpdateStatus(int status)
    {
        var oldStatus = StatusId;
        if (status != oldStatus)
        {
            StatusId = status;

            //AddDomainEvent(new OrderUpdated(Id));
            //AddDomainEvent(new OrderStatusUpdated(Id, status, oldStatus));

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
            //AddDomainEvent(new OrderAssignedUserUpdated(OrderNo, userId, oldAssigneeId));

            return true;
        }

        return false;
    }

    public string? CustomerId { get; set; }

    public bool VatIncluded { get; set; }

    public string Currency { get; set; } = "SEK";
    
    public decimal SubTotal { get; set; } 

    public double VatRate { get; set; } 

    public decimal? Vat { get; set; }

    public decimal Discount { get; set; } 

    public decimal Total { get; set; } 

    public ValueObjects.BillingDetails? BillingDetails { get; set; } = null!;

    public ValueObjects.ShippingDetails? ShippingDetails { get; set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public OrderItem AddOrderItem(string description, string? itemId, string? unit, decimal unitPrice, double vatRate, double quantity, string? notes) 
    {
        var orderItem = new OrderItem(itemId, description, quantity, unit, unitPrice,  unitPrice * (decimal)quantity, vatRate, notes);
        _items.Add(orderItem);
        return orderItem;
    }

    public void RemoveOrderItem(OrderItem orderItem) => _items.Remove(orderItem);

    public void Calculate()
    {
        foreach(var item in Items) 
        {
            item.Total = item.UnitPrice * (decimal)item.Quantity;
        }

        VatRate = 0.25;
        Vat = VatIncluded ? Items.Select(x => x.Total.GetVatFromTotal(x.VatRate)).Sum() : Items.Sum(x => (decimal)x.VatRate * x.Total);
        Total = Items.Sum(x => x.Total);
        SubTotal = (VatIncluded ? (Total - Vat.GetValueOrDefault()) : Total);
    }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
