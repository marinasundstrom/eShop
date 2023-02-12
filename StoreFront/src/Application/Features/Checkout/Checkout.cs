using System;
using System.Text.Json;
using MediatR;
using YourBrand.Inventory;
using YourBrand.Orders;
using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application.Features.Checkout;

public sealed record Checkout(
    BillingDetailsDto BillingDetails,
    ShippingDetailsDto ShippingDetails)
    : IRequest
{
    sealed class Handler : IRequestHandler<Checkout>
    {
        private readonly YourBrand.Orders.IOrdersClient _ordersClient;
        private readonly YourBrand.Carts.ICartsClient cartsClient;
        private readonly IItemsClient productsClient;
        private readonly IWarehouseItemsClient productsClient1;
        private readonly YourBrand.Catalog.IProductsClient productsClient2;
        private readonly ICurrentUserService currentUserService;
        private readonly ICartHubService cartHubService;

        public Handler(
            YourBrand.Orders.IOrdersClient ordersClient,
            YourBrand.Carts.ICartsClient  cartsClient,
            YourBrand.Inventory.IItemsClient productsClient,
            YourBrand.Inventory.IWarehouseItemsClient productsClient1,
            YourBrand.Catalog.IProductsClient productsClient2,
            ICartHubService cartHubService,
            ICurrentUserService currentUserService)
        {
            _ordersClient = ordersClient;
            this.cartsClient = cartsClient;
            this.productsClient = productsClient;
            this.productsClient1 = productsClient1;
            this.productsClient2 = productsClient2;
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
                var item = await productsClient2.GetProductAsync(cartItem.ItemId, cancellationToken);

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

            await _ordersClient.CreateOrderAsync(new YourBrand.Orders.CreateOrderRequest()
            {
                CustomerId = customerId?.ToString(),
                BillingDetails = new Orders.BillingDetailsDto
                {
                    FirstName = request.BillingDetails.FirstName,
                    LastName = request.BillingDetails.LastName,
                    Ssn = request.BillingDetails.SSN,
                    Email = request.BillingDetails.Email,
                    PhoneNumber = request.BillingDetails.PhoneNumber,
                    Address = Map(request.BillingDetails.Address)
                },
                ShippingDetails = new Orders.ShippingDetailsDto
                {
                    FirstName = request.ShippingDetails.FirstName,
                    LastName = request.ShippingDetails.LastName,
                    CareOf = request.ShippingDetails.CareOf,
                    Address = Map(request.ShippingDetails.Address)
                },
                Items = items.ToList()
            }, cancellationToken);

            foreach (var item in items)
            {
                try
                {
                    await productsClient1.ReserveItemsAsync("66189587-67b2-454a-b786-7b49b64fd242", item.ItemId, new ReserveItemsDto() { Quantity = (int)item.Quantity });
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

        private Orders.AddressDto Map(AddressDto address)
        {
            return new()
            {
                Thoroughfare = address.Thoroughfare,
                Premises = address.Premises,
                SubPremises = address.SubPremises,
                PostalCode = address.PostalCode,
                Locality = address.Locality,
                SubAdministrativeArea = address.SubAdministrativeArea,
                AdministrativeArea = address.AdministrativeArea,
                Country = address.Country
            };
        }

        private static decimal CalculatePrice(YourBrand.Catalog.ProductDto item, IEnumerable<Option>? options)
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

