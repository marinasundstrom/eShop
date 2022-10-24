using Microsoft.AspNetCore.Mvc;
using Site.Shared;
using YourBrand.Sales;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ILogger<CartsController> _logger;
    private readonly YourBrand.Sales.ICartsClient _cartsClient;
    private readonly YourBrand.Catalog.IItemsClient _itemsClient;

    public CartsController(ILogger<CartsController> logger, YourBrand.Sales.ICartsClient cartsClient, YourBrand.Catalog.IItemsClient itemsClient)
    {
        _logger = logger;
        _cartsClient = cartsClient;
        _itemsClient = itemsClient;
    }

    [HttpGet]
    public async Task<ItemsResultOfCartDto> GetCarts(YourBrand.Sales.CartStatusDto? status = null, string? assignedTo = null, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _cartsClient.GetCartsAsync(status, assignedTo, page - 1, pageSize, sortBy, sortDirection, cancellationToken);
    }
    
    [HttpGet("{id}")]
    public async Task<SiteCartDto?> GetCart(string id, CancellationToken cancellationToken = default)
    {
        var cart = await _cartsClient.GetCartByIdAsync(id, cancellationToken);
        
        var items = new List<SiteCartItemDto>();

        foreach(var cartItem in cart.Items) 
        {
            var item = await _itemsClient.GetItemAsync(cartItem.ItemId);

            items.Add(new SiteCartItemDto(cartItem.Id, new SiteItemDto(item.Id, item.Name, item.Description, item.Image, item.Price.GetValueOrDefault()), (int)cartItem.Quantity, 0));
        }

        return new SiteCartDto(items);

        /*
        return new CartDto(cart.Items.Select(ci => new CartItemDto()));
        */
    }

    [HttpDelete("{id}")]
    public async Task DeleteCartAsync(string id, CancellationToken cancellationToken = default)
    {
        await _cartsClient.DeleteCartAsync(id, cancellationToken);
    }

    [HttpPost("{id}/Items")]
    public async Task AddItemToCart(string id, AddCartItemDto dto, CancellationToken cancellationToken = default)
    {
        var item = await _itemsClient.GetItemAsync(dto.ItemId);

        if(item.HasVariants) 
        {
            throw new Exception();
        }

        var dto2 = new YourBrand.Sales.CreateCartItemRequest() {
            ItemId = dto.ItemId,
            Quantity = dto.Quantity
        };
        await _cartsClient.AddCartItemAsync(id, dto2, cancellationToken);
    }

    [HttpPut("{id}/Items/{itemId}/Quantity")]
    public async Task UpdateCartItemQuantity(string id, string itemId, int quantity, CancellationToken cancellationToken = default)
    {
        await _cartsClient.UpdateCartItemQuantityAsync(id, itemId, quantity, cancellationToken);
    }

    [HttpDelete("{id}/Items/{itemId}")]
    public async Task RemoveItemFromCart(string id, string itemId, CancellationToken cancellationToken = default)
    {
        await _cartsClient.RemoveCartItemAsync(id, itemId, cancellationToken);
    }
}

public record AddCartItemDto(string? ItemId, int Quantity);

public record SiteItemDto(string Id, string Name, string? Description, string? Image, decimal Price);

public record SiteCartDto(IEnumerable<SiteCartItemDto> Items);

public record SiteCartItemDto(string Id, SiteItemDto Item, int Quantity, decimal Total);