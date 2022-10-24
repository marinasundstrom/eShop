using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Sales.Application;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Carts.Items.Commands;
using YourBrand.Sales.Application.Carts.Dtos;
using YourBrand.Sales.Application.Carts.Queries;

namespace YourBrand.Sales.Presentation.Controllers;

partial class CartsController
{
    [HttpPost("{id}/Items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CartItemDto>> AddCartItem(string id, CreateCartItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AddCartItem(id, request.ItemId, request.Quantity), cancellationToken);
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

    [HttpDelete("{id}/Items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> RemoveCartItem(string id, string itemId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RemoveCartItem(id, itemId), cancellationToken);
        return this.HandleResult(result);
    }
}

public sealed record CreateCartItemRequest(string? ItemId, double Quantity);