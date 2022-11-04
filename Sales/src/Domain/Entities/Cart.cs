using System.Collections.Generic;
using YourBrand.Sales.Domain.Events;

namespace YourBrand.Sales.Domain.Entities;

public class Cart : AggregateRoot<string>, IAuditable
{
    HashSet<CartItem> _items = new HashSet<CartItem>();

    public Cart(string? tag)
        : base(Guid.NewGuid().ToString())
    {
        Tag = tag;
    }

    public string? Tag { get; set; }

    public IReadOnlyCollection<CartItem> Items => _items;

    public CartItem AddCartItem(string? itemId, double quantity, string? data) 
    {
        CartItem? cartItem = _items.FirstOrDefault(x => x.ItemId == itemId && x.Data == data);
        if(cartItem is null) 
        {
            cartItem = new CartItem(itemId, quantity, data);
            _items.Add(cartItem);
        }
        else 
        {
            cartItem.AddToQuantity(quantity);
        }
        return cartItem;
    }

    public void RemoveCartItem(CartItem cartItem) => _items.Remove(cartItem);

    public void Clear()
    {
        _items.Clear();
    }

    public void Checkout()
    {
        foreach(var item in _items) 
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
