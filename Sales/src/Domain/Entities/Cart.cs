using System.Collections.Generic;
using YourBrand.Sales.Domain.Enums;
using YourBrand.Sales.Domain.Events;

namespace YourBrand.Sales.Domain.Entities;

public class Cart : AuditableEntity, IAggregateRoot
{
    HashSet<CartItem> _items = new HashSet<CartItem>();

    protected Cart()
    {
    }

    public Cart(string userId)
    {
    }

    public string Id { get; private set; } = Guid.NewGuid().ToString();

    public IReadOnlyCollection<CartItem> Items => _items;

    public CartItem AddCartItem(string description, string? itemId, decimal price, double quantity, decimal total) 
    {
        var cartItem = new CartItem(description, itemId, price, quantity, total);
        _items.Add(cartItem);
        return cartItem;
    }

    public void RemoveCartItem(CartItem cartItem) => _items.Remove(cartItem);
}
