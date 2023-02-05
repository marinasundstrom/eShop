using System;
using MediatR;
using YourBrand.Carts.Application.Common;
using YourBrand.Carts.Application.Services;

namespace YourBrand.Carts.Application.Features.Carts.EventHandlers;

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

