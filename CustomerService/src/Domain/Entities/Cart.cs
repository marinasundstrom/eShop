using System.Collections.Generic;
using YourBrand.CustomerService.Domain.Events;

namespace YourBrand.CustomerService.Domain.Entities;

public class Issue : AggregateRoot<string>, IAuditable
{
    HashSet<IssueItem> _items = new HashSet<IssueItem>();

    public Issue(string? tag)
        : base(Guid.NewGuid().ToString())
    {
        Tag = tag;
    }

    public string? Tag { get; set; }

    public IReadOnlyCollection<IssueItem> Items => _items;

    public IssueItem AddIssueItem(string? itemId, double quantity, string? data)
    {
        IssueItem? issueItem = _items.FirstOrDefault(x => x.ItemId == itemId && x.Data == data);
        if (issueItem is null)
        {
            issueItem = new IssueItem(itemId, quantity, data);
            _items.Add(issueItem);
        }
        else
        {
            issueItem.AddToQuantity(quantity);
        }
        return issueItem;
    }

    public void RemoveIssueItem(IssueItem issueItem) => _items.Remove(issueItem);

    public void Clear()
    {
        _items.Clear();
    }

    public void Checkout()
    {
        foreach (var item in _items)
        {
            item.CheckedOutAt = DateTimeOffset.UtcNow;
        }
    }

    public User? CreatedBy { get; set; }

    public string? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
