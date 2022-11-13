using YourBrand.Carts.Application.Common;
using YourBrand.Carts.Application.Services;
using YourBrand.Carts.Domain.Entities;

namespace YourBrand.Carts.Application.Carts.EventHandlers;

public sealed class CartDeletedEventHandler : IDomainEventHandler<CartDeleted>
{
    private readonly ICartRepository cartRepository;

    public CartDeletedEventHandler(ICartRepository cartRepository)
    {
        this.cartRepository = cartRepository;
    }

    public async Task Handle(CartDeleted notification, CancellationToken cancellationToken)
    {
    }
}

