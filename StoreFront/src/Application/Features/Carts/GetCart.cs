﻿using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using YourBrand.Orders;

namespace YourBrand.StoreFront.Application.Features.Carts;

public sealed record GetCart : IRequest<SiteCartDto>
{
    sealed class Handler : IRequestHandler<GetCart, SiteCartDto>
    {
        private readonly IStoresProvider _storesProvider;
        private readonly YourBrand.Carts.ICartsClient  _cartsClient;
        private readonly YourBrand.Catalog.IProductsClient _productsClient;
        private readonly ICurrentUserService currentUserService;
        private readonly IDistributedCache cache;

        public Handler(
            IStoresProvider storesProvider,
            YourBrand.Carts.ICartsClient  cartsClient,
            YourBrand.Catalog.IProductsClient productsClient,
            ICurrentUserService currentUserService,
            IDistributedCache cache)
        {
            _storesProvider = storesProvider;
            _cartsClient = cartsClient;
            _productsClient = productsClient;
            this.currentUserService = currentUserService;
            this.cache = cache;
        }

        public async Task<SiteCartDto> Handle(GetCart request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;
            var clientId = currentUserService.ClientId;

            var store = await _storesProvider.GetCurrentStore(cancellationToken);

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

                            return await _productsClient.GetProductAsync(cartItem.ItemId!, cancellationToken);
                        });

                if(string.IsNullOrEmpty(cartItem.Data)) 
                {
                    cartItem.Data = "[]";
                }

                var options = JsonSerializer.Deserialize<IEnumerable<Option>>(cartItem.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                })!;

                var price = item!.Price;
                var regularPrice = item.RegularPrice;

                List<string> optionTexts = new List<string>();

                foreach (var option in options)
                {
                    var opt = item.Options.FirstOrDefault(x => x.Option.Id == option.Id);

                    if (opt is not null)
                    {
                        if (option.OptionType == 0)
                        {
                            var isSelected = option.IsSelected.GetValueOrDefault();

                            if (!isSelected && isSelected != opt.Option.IsSelected)
                            {
                                optionTexts.Add($"No {option.Name}");

                                continue;
                            }

                            if (isSelected)
                            {
                                price += option.Price.GetValueOrDefault();
                                regularPrice += option.Price.GetValueOrDefault();

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
                            var value = opt.Option.Values.FirstOrDefault(x => x.Id == option.SelectedValueId)!;

                            price += value.Price.GetValueOrDefault();
                            regularPrice += value.Price.GetValueOrDefault();

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
                            //regularPrice += option.Price.GetValueOrDefault();

                            optionTexts.Add($"{option.NumericalValue} {option.Name}");
                        }
                    }
                }

                items.Add(new SiteCartItemDto(cartItem.Id, item.ToDto(store!, string.Join(", ", optionTexts)), (int)cartItem.Quantity, (decimal)cartItem.Quantity * price.GetValueOrDefault(), cartItem.Data));
            }

            return new SiteCartDto(cart.Id, items);
        }
    }
}

