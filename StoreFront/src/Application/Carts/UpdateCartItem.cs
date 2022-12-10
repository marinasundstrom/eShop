using MediatR;
using YourBrand.Catalog;
using YourBrand.Sales;

namespace YourBrand.StoreFront.Application.Carts;

public sealed record UpdateCartItem(string Id, int Quantity, string? Data) : IRequest
{
    sealed class Handler : IRequestHandler<UpdateCartItem>
    {
        private readonly YourBrand.Carts.ICartsClient cartsClient;
        private readonly IProductsClient itemsClient;
        private readonly ICartHubService cartHubService;
        private readonly ICurrentUserService currentUserService;

        public Handler(
            YourBrand.Carts.ICartsClient  cartsClient,
            YourBrand.Catalog.IProductsClient itemsClient,
            ICartHubService cartHubService,
            ICurrentUserService currentUserService)
        {
            this.cartsClient = cartsClient;
            this.itemsClient = itemsClient;
            this.cartHubService = cartHubService;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateCartItem request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;
            var clientId = currentUserService.ClientId;

            string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

            var cart = await cartsClient.GetCartByTagAsync(tag, cancellationToken);

            var cartItem = cart.Items.First(x => x.Id == request.Id);

            await cartsClient.UpdateCartItemQuantityAsync(cart.Id, cartItem.Id, request.Quantity, cancellationToken);

            await cartsClient.UpdateCartItemDataAsync(cart.Id, cartItem.Id, request.Data, cancellationToken);

            await cartHubService.UpdateCart();

            return Unit.Value;
        }
    }
}

