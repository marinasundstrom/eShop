using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Carts.Application;
using YourBrand.Carts.Application.Common;
using YourBrand.Carts.Application.Features.Carts.Items.Commands;
using YourBrand.Carts.Application.Features.Carts.Dtos;
using YourBrand.Carts.Application.Features.Carts.Queries;

namespace YourBrand.Carts.Application.Features.Carts;

partial class CartsController
{
    [HttpPost("{id}/Items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartItemDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CartItemDto>> AddCartItem(string id, CreateCartItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AddCartItem(id, request.ItemId, request.Quantity, request.Data), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpPut("{id}/Items/{itemId}/Quantity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateCartItemQuantity(string id, string itemId, double quantity, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateCartItemQuantity(id, itemId, quantity), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/Items/{itemId}/Data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateCartItemData(string id, string itemId, string? data, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateCartItemData(id, itemId, data), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpDelete("{id}/Items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> RemoveCartItem(string id, string itemId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveCartItem(id, itemId), cancellationToken);
        return this.HandleResult(result);
    }
}

public sealed record CreateCartItemRequest(string? ItemId, double Quantity, string? Data);