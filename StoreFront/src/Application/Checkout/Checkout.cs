using System;
using System.Text.Json;
using MediatR;
using YourBrand.Inventory;
using YourBrand.Sales;
using YourBrand.StoreFront.Application.Carts;

namespace YourBrand.StoreFront.Application.Checkout;

public sealed record Checkout(
    YourBrand.Sales.BillingDetailsDto BillingDetails,
    YourBrand.Sales.ShippingDetailsDto ShippingDetails)
    : IRequest
{
    sealed class Handler : IRequestHandler<Checkout>
    {
        private readonly YourBrand.Sales.IOrdersClient _ordersClient;
        private readonly YourBrand.Carts.ICartsClient cartsClient;
        private readonly IItemsClient itemsClient;
        private readonly IWarehouseItemsClient itemsClient1;
        private readonly YourBrand.Catalog.IItemsClient itemsClient2;
        private readonly ICurrentUserService currentUserService;
        private readonly ICartHubService cartHubService;

        public Handler(
            YourBrand.Sales.IOrdersClient ordersClient,
            YourBrand.Carts.ICartsClient  cartsClient,
            YourBrand.Inventory.IItemsClient itemsClient,
            YourBrand.Inventory.IWarehouseItemsClient itemsClient1,
            YourBrand.Catalog.IItemsClient itemsClient2,
            ICartHubService cartHubService,
            ICurrentUserService currentUserService)
        {
            _ordersClient = ordersClient;
            this.cartsClient = cartsClient;
            this.itemsClient = itemsClient;
            this.itemsClient1 = itemsClient1;
            this.itemsClient2 = itemsClient2;
            this.currentUserService = currentUserService;
            this.cartHubService = cartHubService;
        }

        public async Task<Unit> Handle(Checkout request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;
            var clientId = currentUserService.ClientId;

            string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

            var cart = await cartsClient.GetCartByTagAsync(tag);

            var items = new List<CreateOrderItemDto>();

            foreach (var cartItem in cart.Items)
            {
                var item = await itemsClient2.GetItemAsync(cartItem.ItemId, cancellationToken);

                var options = JsonSerializer.Deserialize<IEnumerable<Option>>(cartItem.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                })!;

                decimal price = item.Price;

                price += CalculatePrice(item, options);

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
                            var value = opt.Values.FirstOrDefault(x => x.Id == option.SelectedValueId);

                            if (value?.Price is not null)
                            {
                                optionTexts.Add($"{value.Name} (+{value.Price?.ToString("c")})");
                            }
                            else
                            {
                                optionTexts.Add(value!.Name);
                            }
                        }
                        else if (option.NumericalValue is not null)
                        {
                            optionTexts.Add($"{option.NumericalValue} {option.Name}");
                        }
                    }
                }

                /*
                var price = item.Price
                    + options
                    .Where(x => x.IsSelected.GetValueOrDefault() || x.SelectedValueId is not null)
                    .Select(x => x.Price.GetValueOrDefault() + (x.Values.FirstOrDefault(x3 => x3.Id == x?.SelectedValueId)?.Price ?? 0))
                    .Sum();
                    */

                items.Add(new CreateOrderItemDto
                {
                    Description = item.Name,
                    ItemId = cartItem.ItemId,
                    Notes = string.Join(", ", optionTexts),
                    UnitPrice = price,
                    VatRate = 0.25,
                    Quantity = cartItem.Quantity
                });
            }

            await _ordersClient.CreateOrderAsync(new YourBrand.Sales.CreateOrderRequest()
            {
                CustomerId = customerId?.ToString(),
                BillingDetails = request.BillingDetails,
                ShippingDetails = request.ShippingDetails,
                Items = items.ToList()
            }, cancellationToken);

            foreach (var item in items)
            {
                try
                {
                    await itemsClient1.ReserveItemsAsync("66189587-67b2-454a-b786-7b49b64fd242", item.ItemId, new ReserveItemsDto() { Quantity = (int)item.Quantity });
                }
                catch (Exception e)
                {
                }
            }

            await cartsClient.CheckoutAsync(cart.Id);
            await cartsClient.ClearCartAsync(cart.Id);

            await cartHubService.UpdateCart();

            return Unit.Value;
        }

        private static decimal CalculatePrice(YourBrand.Catalog.ItemDto item, IEnumerable<Option>? options)
        {
            decimal price = 0;

            foreach (var option in options!
                .Where(x => x.IsSelected.GetValueOrDefault() || x.SelectedValueId is not null))
            {
                var o = item.Options.FirstOrDefault(x => x.Id == option.Id);
                if (o is not null)
                {
                    if (option.IsSelected.GetValueOrDefault())
                    {
                        price += option.Price.GetValueOrDefault();
                    }
                    else if (option.SelectedValueId is not null)
                    {
                        var sVal = o.Values.FirstOrDefault(x => x.Id == option.SelectedValueId);
                        if (sVal is not null)
                        {
                            price += sVal.Price.GetValueOrDefault();
                        }
                    }
                }
            }

            return price;
        }
    }
}

