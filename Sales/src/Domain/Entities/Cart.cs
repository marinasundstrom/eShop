using System.Collections.Generic;
using YourBrand.Sales.Domain.Events;

namespace YourBrand.Sales.Domain.Entities;

public class Cart : AuditableEntity, IAggregateRoot
{
    HashSet<CartItem> _items = new HashSet<CartItem>();

    protected Cart()
    {
    }

    public Cart(string? tag)
    {
        Tag = tag;
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

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
}
