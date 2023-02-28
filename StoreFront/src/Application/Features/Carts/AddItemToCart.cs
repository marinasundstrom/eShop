using System.Text.Json;

using MediatR;
using YourBrand.Catalog;
using YourBrand.Orders;

namespace YourBrand.StoreFront.Application.Features.Carts;

public sealed record AddItemToCart(string ItemId, int Quantity, string? Data) : IRequest
{
    sealed class Handler : IRequestHandler<AddItemToCart>
    {
        private readonly YourBrand.Carts.ICartsClient cartsClient;
        private readonly IProductsClient productsClient;
        private readonly ICartHubService cartHubService;
        private readonly ICurrentUserService currentUserService;

        public Handler(
            YourBrand.Carts.ICartsClient  cartsClient,
            YourBrand.Catalog.IProductsClient productsClient,
            ICartHubService cartHubService,
            ICurrentUserService currentUserService)
        {
            this.cartsClient = cartsClient;
            this.productsClient = productsClient;
            this.cartHubService = cartHubService;
            this.currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(AddItemToCart request, CancellationToken cancellationToken)
        {
            var item = await productsClient.GetProductAsync(request.ItemId);

            if (item.HasVariants)
            {
                throw new Exception();
            }

            string? data = request.Data;

            if(string.IsNullOrEmpty(data)) 
            {
                var dataArray = item.Options.Select(x =>
                {
                    return new Option
                    {
                        Id = x.Option.Id,
                        Name = x.Option.Name,
                        OptionType = (int)x.Option.OptionType,
                        ProductId = x.Option.ProductId,
                        Price = x.Option.Price,
                        IsSelected = x.Option.IsSelected,
                        SelectedValueId = x.Option.DefaultValue?.Id,
                        NumericalValue = x.Option.DefaultNumericalValue,
                        TextValue = x.Option.DefaultTextValue
                    };
                });
                
                data = JsonSerializer.Serialize(dataArray, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });
            }

            var dto2 = new YourBrand.Carts.CreateCartItemRequest()
            {
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                Data = data
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

