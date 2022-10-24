using MediatR;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Services;

namespace YourBrand.Sales.Application.Carts.EventHandlers;

public sealed class CartUpdatedEventHandler : IDomainEventHandler<CartUpdated>
{
    private readonly ICartRepository cartRepository;

    public CartUpdatedEventHandler(ICartRepository cartRepository)
    {
        this.cartRepository = cartRepository;
    }

    public async Task Handle(CartUpdated notification, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.FindByIdAsync(notification.CartId, cancellationToken);

        if (cart is null)
            return;
    }
}
