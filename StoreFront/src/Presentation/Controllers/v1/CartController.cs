using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using YourBrand.StoreFront.Application.Services;
using YourBrand.Sales;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using MediatR;
using YourBrand.StoreFront.Application.Carts;

namespace YourBrand.StoreFront.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class CartController : ControllerBase
{
    private readonly ILogger<CartController> _logger;
    private readonly IMediator mediator;

    public CartController(
        ILogger<CartController> logger,
        IMediator mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }
    
    [HttpGet]
    public async Task<SiteCartDto?> GetCart(CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetCart());
    }

    [HttpPost("Items")]
    public async Task AddItemToCart(AddCartItemDto dto, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new AddItemToCart(dto.ItemId!, dto.Quantity, dto.Data), cancellationToken);
    }

    [HttpPut("Items/{id}")]
    public async Task UpdateCartItem(string id, UpdateCartItemDto dto, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UpdateCartItem(id, dto.Quantity, dto.Data), cancellationToken);
    }

    [HttpPut("Items/{itemId}/Quantity")]
    public async Task UpdateCartItemQuantity(string itemId, int quantity, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new UpdateCartItemQuantity(itemId, quantity), cancellationToken);
    }

    [HttpDelete("Items/{itemId}")]
    public async Task RemoveItemFromCart(string itemId, CancellationToken cancellationToken = default)
    {
        await mediator.Send(new RemoveItemFromCart(itemId), cancellationToken);
    }

    [HttpDelete("Items")]
    public async Task ClearCart(CancellationToken cancellationToken = default)
    {
        await mediator.Send(new ClearCart(), cancellationToken);
    }
}
