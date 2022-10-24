using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Services;
using YourBrand.Sales.Domain.Entities;

namespace YourBrand.Sales.Application.Carts.EventHandlers;

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

