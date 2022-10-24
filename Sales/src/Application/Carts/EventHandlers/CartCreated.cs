using System;
using MediatR;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Services;

namespace YourBrand.Sales.Application.Carts.EventHandlers;

public sealed class CartCreatedEventHandler : IDomainEventHandler<CartCreated>
{
    private readonly ICartRepository cartRepository;

    public CartCreatedEventHandler(ICartRepository cartRepository)
    {
        this.cartRepository = cartRepository;
    }

    public async Task Handle(CartCreated notification, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.FindByIdAsync(notification.CartId, cancellationToken);

        if (cart is null)
            return;
    }
}

