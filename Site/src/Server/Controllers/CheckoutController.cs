using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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

            items.Add(new CreateOrderItemDto {
                    Description = item.Description,
                    ItemId = cartItem.ItemId,
                    UnitPrice = item.Price,
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
}

public class CheckoutDto 
{
    public YourBrand.Sales.BillingDetailsDto BillingDetails { get; set; } = null!;

    public YourBrand.Sales.ShippingDetailsDto ShippingDetails { get; set; } = null!;
}