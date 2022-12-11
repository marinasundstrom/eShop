using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using YourBrand.Orders;

namespace YourBrand.StoreFront.Application.Carts;

public sealed record GetCart : IRequest<SiteCartDto>
{
    sealed class Handler : IRequestHandler<GetCart, SiteCartDto>
    {
        private readonly YourBrand.Carts.ICartsClient  _cartsClient;
        private readonly YourBrand.Catalog.IProductsClient _itemsClient;
        private readonly ICurrentUserService currentUserService;
        private readonly IDistributedCache cache;

        public Handler(
            YourBrand.Carts.ICartsClient  cartsClient,
            YourBrand.Catalog.IProductsClient itemsClient,
            ICurrentUserService currentUserService,
            IDistributedCache cache)
        {
            _cartsClient = cartsClient;
            _itemsClient = itemsClient;
            this.currentUserService = currentUserService;
            this.cache = cache;
        }

        public async Task<SiteCartDto> Handle(GetCart request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;
            var clientId = currentUserService.ClientId;

            //Console.WriteLine(currentUserService.Host);

            YourBrand.Carts.CartDto cart;

            string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

            try
            {
                cart = await _cartsClient.GetCartByTagAsync(tag, cancellationToken);
            }
            catch
            {
                var request2 = new YourBrand.Carts.CreateCartRequest
                {
                    Tag = tag
                };
                cart = await _cartsClient.CreateCartAsync(request2, cancellationToken);
            }

            var items = new List<SiteCartItemDto>();

            foreach (var cartItem in cart.Items)
            {
                var item = await cache.GetOrCreateAsync(
                        $"item-{cartItem.ItemId}", async (options, cancellationToken) =>
                        {
                            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);

                            return await _itemsClient.GetProductAsync(cartItem.ItemId, cancellationToken);
                        });

                var options = JsonSerializer.Deserialize<IEnumerable<Option>>(cartItem.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                })!;

                var price = item!.Price;
                var compareAtPrice = item.CompareAtPrice;

                List<string> optionTexts = new List<string>();

                foreach (var option in options)
                {
                    var opt = item.Options.FirstOrDefault(x => x.Id == option.Id);

                    if (opt is not null)
                    {
                        if (option.OptionType == 0)
                        {
                            var isSelected = option.IsSelected.GetValueOrDefault();

                            if (!isSelected && isSelected != opt.IsSelected)
                            {
                                optionTexts.Add($"No {option.Name}");

                                continue;
                            }

                            if (isSelected)
                            {
                                price += option.Price.GetValueOrDefault();
                                compareAtPrice += option.Price.GetValueOrDefault();

                                if (option.Price is not null)
                                {
                                    optionTexts.Add($"{option.Name} (+{option.Price?.ToString("c")})");
                                }
                                else
                                {
                                    optionTexts.Add(option.Name);
                                }
                            }
                        }
                        else if (option.SelectedValueId is not null)
                        {
                            var value = opt.Values.FirstOrDefault(x => x.Id == option.SelectedValueId)!;

                            price += value.Price.GetValueOrDefault();
                            compareAtPrice += value.Price.GetValueOrDefault();

                            if (value.Price is not null)
                            {
                                optionTexts.Add($"{value.Name} (+{value.Price?.ToString("c")})");
                            }
                            else
                            {
                                optionTexts.Add(value.Name);
                            }
                        }
                        else if (option.NumericalValue is not null)
                        {
                            //price += option.Price.GetValueOrDefault();
                            //compareAtPrice += option.Price.GetValueOrDefault();

                            optionTexts.Add($"{option.NumericalValue} {option.Name}");
                        }
                    }
                }

                items.Add(new SiteCartItemDto(cartItem.Id, item.ToDto(string.Join(", ", optionTexts)), (int)cartItem.Quantity, (decimal)cartItem.Quantity * price, cartItem.Data));
            }

            return new SiteCartDto(cart.Id, items);
        }
    }
}

