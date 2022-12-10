using MediatR;
using YourBrand.Catalog;
using YourBrand.Sales;

namespace YourBrand.StoreFront.Application.Carts;

public sealed record AddItemToCart(string ItemId, int Quantity, string? Data) : IRequest
{
    sealed class Handler : IRequestHandler<AddItemToCart>
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

        public async Task<Unit> Handle(AddItemToCart request, CancellationToken cancellationToken)
        {
            var item = await itemsClient.GetProductAsync(request.ItemId);

            if (item.HasVariants)
            {
                throw new Exception();
            }

            var dto2 = new YourBrand.Carts.CreateCartItemRequest()
            {
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                Data = request.Data
            };

            var customerId = currentUserService.CustomerNo;
            var clientId = currentUserService.ClientId;

            string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

            var cart = await cartsClient.GetCartByTagAsync(tag, cancellationToken);

            await cartsClient.AddCartItemAsync(cart.Id, dto2, cancellationToken);

            await cartHubService.UpdateCart();

            return Unit.Value;
        }
    }
}

