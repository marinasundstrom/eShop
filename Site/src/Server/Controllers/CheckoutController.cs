using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using Site.Server.Hubs;
using YourBrand.Inventory;
using YourBrand.Sales;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly ILogger<CheckoutController> _logger;
    private readonly YourBrand.Sales.IOrdersClient _ordersClient;
    private readonly ICartsClient cartsClient;
    private readonly IItemsClient itemsClient;
    private readonly YourBrand.Catalog.IItemsClient itemsClient2;
    private readonly IHubContext<CartHub, ICartHubClient> cartHubContext;

    public CheckoutController(
        ILogger<CheckoutController> logger, 
        YourBrand.Sales.IOrdersClient ordersClient, 
        YourBrand.Sales.ICartsClient cartsClient,
        YourBrand.Inventory.IItemsClient itemsClient,
        YourBrand.Catalog.IItemsClient itemsClient2,
        IHubContext<CartHub, ICartHubClient> cartHubContext)
    {
        _logger = logger;
        _ordersClient = ordersClient;
        this.cartsClient = cartsClient;
        this.itemsClient = itemsClient;
        this.itemsClient2 = itemsClient2;
        this.cartHubContext = cartHubContext;
    }

    [HttpPost]
    public async Task Checkout(CheckoutDto dto, CancellationToken cancellationToken = default)
    {
        string customerId = "1";

        var cart = await cartsClient.GetCartByIdAsync("test");

        var items = new List<CreateOrderItemDto>();

        foreach(var cartItem in cart.Items)
        {
            var item = await itemsClient2.GetItemAsync(cartItem.ItemId, cancellationToken);

            var options = JsonSerializer.Deserialize<IEnumerable<Option>>(cartItem.Data, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            decimal price = item.Price;

            price += CalculatePrice(item, options);

            /*
            var price = item.Price
                + options
                .Where(x => x.IsSelected.GetValueOrDefault() || x.SelectedValueId is not null)
                .Select(x => x.Price.GetValueOrDefault() + (x.Values.FirstOrDefault(x3 => x3.Id == x?.SelectedValueId)?.Price ?? 0))
                .Sum();
                */

            items.Add(new CreateOrderItemDto
            {
                Description = $"{item.Name} - {item.Description}",
                ItemId = cartItem.ItemId,
                UnitPrice = price,
                VatRate = 0.25,
                Quantity = cartItem.Quantity
            });
        }

        await _ordersClient.CreateOrderAsync(new YourBrand.Sales.CreateOrderRequest() {
            CustomerId = customerId,
            BillingDetails = dto.BillingDetails,
            ShippingDetails = dto.ShippingDetails,
            Items = items.ToList()
        }, cancellationToken);

        await cartsClient.ClearCartAsync(cart.Id);

        await cartHubContext.Clients.All.CartUpdated();
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

public class CheckoutDto 
{
    public YourBrand.Sales.BillingDetailsDto BillingDetails { get; set; } = null!;

    public YourBrand.Sales.ShippingDetailsDto ShippingDetails { get; set; } = null!;
}