using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Site.Server.Hubs;
using Site.Server.Services;
using YourBrand.Sales;
using Microsoft.Extensions.Caching.Memory;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;
    private readonly YourBrand.Sales.ICartsClient _cartsClient;
    private readonly YourBrand.Catalog.IItemsClient _itemsClient;
    private readonly IHubContext<CartHub, ICartHubClient> _cartHubContext;
    private readonly ICurrentUserService currentUserService;
    private readonly IMemoryCache memoryCache;

    public CartController(
        ILogger<CartController> logger, 
        YourBrand.Sales.ICartsClient cartsClient, 
        YourBrand.Catalog.IItemsClient itemsClient,
        IHubContext<CartHub, ICartHubClient> cartHubContext,
        ICurrentUserService currentUserService,
        IMemoryCache memoryCache)
    {
        _logger = logger;
        _cartsClient = cartsClient;
        _itemsClient = itemsClient;
        _cartHubContext = cartHubContext;
        this.currentUserService = currentUserService;
        this.memoryCache = memoryCache;
    }
    
    [HttpGet]
    public async Task<SiteCartDto?> GetCart(CancellationToken cancellationToken = default)
    {
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;

        CartDto cart;
        
        string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

        try 
        {
            cart = await _cartsClient.GetCartByTagAsync(tag, cancellationToken);
        } 
        catch
        {
            var request = new CreateCartRequest {
                Tag = tag
            };
            cart = await _cartsClient.CreateCartAsync(request, cancellationToken);
        }
        
        var items = new List<SiteCartItemDto>();

        foreach(var cartItem in cart.Items) 
        {
            var item =  await memoryCache.GetOrCreateAsync(
                    $"item-{cartItem.ItemId}", async options =>
                    {
                        options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);

                        return await _itemsClient.GetItemAsync(cartItem.ItemId, cancellationToken);
                    });

            var options = JsonSerializer.Deserialize<IEnumerable<Option>>(cartItem.Data, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            var price = item.Price;
            var compareAtPrice = item.CompareAtPrice;

            List<string> optionTexts = new List<string>();

            foreach(var option in options) 
            {
                var opt = item.Options.FirstOrDefault(x => x.Id == option.Id);

                if(opt is not null) 
                {
                    if(option.OptionType == 0) 
                    {
                        var isSelected = option.IsSelected.GetValueOrDefault();

                        if(!isSelected && isSelected != opt.IsSelected) 
                        {
                            optionTexts.Add($"No {option.Name}");

                            continue;
                        }

                        if(isSelected)
                        {
                            price += option.Price.GetValueOrDefault();
                            compareAtPrice += option.Price.GetValueOrDefault();

                            if(option.Price is not null) 
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
                        
                        price += value.Price.GetValueOrDefault();
                        compareAtPrice += value.Price.GetValueOrDefault();

                        if(value.Price is not null) 
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

        /*
        return new CartDto(cart.Items.Select(ci => new CartItemDto()));
        */
    }

    [HttpPost("Items")]
    public async Task AddItemToCart(AddCartItemDto dto, CancellationToken cancellationToken = default)
    {
        var item = await _itemsClient.GetItemAsync(dto.ItemId);

        if(item.HasVariants) 
        {
            throw new Exception();
        }

        var dto2 = new YourBrand.Sales.CreateCartItemRequest() {
            ItemId = dto.ItemId,
            Quantity = dto.Quantity,
            Data = dto.Data
        };
        
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;
        
        string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

        var cart = await _cartsClient.GetCartByTagAsync(tag, cancellationToken);

        await _cartsClient.AddCartItemAsync(cart.Id, dto2, cancellationToken);

        await UpdateCart();
    }

    private async Task UpdateCart() 
    {
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;
        
        var hubClient = customerId is not null 
            ? _cartHubContext.Clients.Group($"customer-{customerId}") 
            : _cartHubContext.Clients.Group($"cart-{clientId}");
        
        await hubClient.CartUpdated();
    }

    [HttpPut("Items/{id}")]
    public async Task UpdateCartItem(string id, UpdateCartItemDto dto, CancellationToken cancellationToken = default)
    {
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;
        
        string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

        var cart = await _cartsClient.GetCartByTagAsync(tag, cancellationToken);

        var cartItem = cart.Items.First(x => x.Id == id);

        await _cartsClient.UpdateCartItemQuantityAsync(cart.Id, cartItem.Id, dto.Quantity, cancellationToken);

        await _cartsClient.UpdateCartItemDataAsync(cart.Id, cartItem.Id, dto.Data, cancellationToken);

        await UpdateCart();
    }

    [HttpPut("Items/{itemId}/Quantity")]
    public async Task UpdateCartItemQuantity(string itemId, int quantity, CancellationToken cancellationToken = default)
    {
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;
        
        string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

        var cart = await _cartsClient.GetCartByTagAsync(tag, cancellationToken);

        await _cartsClient.UpdateCartItemQuantityAsync(cart.Id, itemId, quantity, cancellationToken);

        await UpdateCart();
    }

    [HttpDelete("Items/{itemId}")]
    public async Task RemoveItemFromCart(string itemId, CancellationToken cancellationToken = default)
    {
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;
        
        string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

        var cart = await _cartsClient.GetCartByTagAsync(tag, cancellationToken);

        await _cartsClient.RemoveCartItemAsync(cart.Id, itemId, cancellationToken);

        await UpdateCart();
    }

    [HttpDelete("Items")]
    public async Task ClearCart(CancellationToken cancellationToken = default)
    {
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;
        
        string tag = customerId is null ? $"cart-{clientId}" : $"cart-{customerId}";

        var cart = await _cartsClient.GetCartByTagAsync(tag, cancellationToken);

        await _cartsClient.ClearCartAsync(cart.Id, cancellationToken);

        await UpdateCart();
    }
}

public record AddCartItemDto(string? ItemId, int Quantity, string? Data);

public record UpdateCartItemDto(int Quantity, string? Data);

public record SiteItemDto(string Id, string Name, string? Description, SiteParentItemDto? Parent, SiteItemGroupDto? Group, string? Image, decimal Price, decimal? CompareAtPrice, int? Available, IEnumerable<YourBrand.Catalog.AttributeDto> Attributes, IEnumerable<YourBrand.Catalog.OptionDto> Options, bool HasVariants, IEnumerable<YourBrand.Catalog.ItemVariantAttributeDto> VariantAttributes);

public record SiteParentItemDto(string Id, string Name, string? Description, SiteItemGroupDto? Group);

public record SiteItemGroupDto(string Id, string Name, SiteItemGroupDto? Parent);

public record SiteCartDto(string Id, IEnumerable<SiteCartItemDto> Items);

public record SiteCartItemDto(string Id, SiteItemDto Item, int Quantity, decimal Total, string? Data);

public class Option
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int OptionType { get; set; }

    public string? ItemId { get; set; }

    public decimal? Price { get; set; }

    public string? TextValue { get; set; }

    public int? NumericalValue { get; set; }

    public bool? IsSelected { get; set; }

    public string? SelectedValueId { get; set; }
}