using System.Collections.Generic;
using YourBrand.Payments.Domain.Events;

namespace YourBrand.Payments.Domain.Entities;

public class Receipt : AggregateRoot<string>, IAuditable
{
    HashSet<ReceiptItem> _items = new HashSet<ReceiptItem>();

    public Receipt() : base(Guid.NewGuid().ToString())
    {
        StatusId = 1;
    }

    public int ReceiptNo { get; set; }

    public string CompanyId { get; private set; } = "ACME";

    public DateTime Date { get; private set; } = DateTime.Now;

    public ReceiptStatus Status { get; private set; } = null!;

    public int StatusId { get; set; } = 1;

    public bool UpdateStatus(int status)
    {
        var oldStatus = StatusId;
        if (status != oldStatus)
        {
            StatusId = status;

            //AddDomainEvent(new ReceiptUpdated(Id));
            //AddDomainEvent(new ReceiptStatusUpdated(Id, status, oldStatus));

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
            //AddDomainEvent(new ReceiptAssignedUserUpdated(ReceiptNo, userId, oldAssigneeId));

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

    public IReadOnlyCollection<ReceiptItem> Items => _items;

    public ReceiptItem AddReceiptItem(string description, string? itemId, string? unit, decimal unitPrice, double vatRate, double quantity, string? notes)
    {
        var orderItem = new ReceiptItem(itemId, description, quantity, unit, unitPrice, unitPrice * (decimal)quantity, vatRate, notes);
        _items.Add(orderItem);
        return orderItem;
    }

    public void RemoveReceiptItem(ReceiptItem orderItem) => _items.Remove(orderItem);

    public void Calculate()
    {
        foreach (var item in Items)
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
