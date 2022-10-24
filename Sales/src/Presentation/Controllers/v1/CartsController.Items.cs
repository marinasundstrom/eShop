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
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CartItemDto>> CreateCartItem(string id, CreateCartItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCartItem(id, request.Description, request.ItemId, request.Price, request.Quantity, request.Total), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}/Items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteCartItem(string id, string itemId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteCartItem(id, itemId), cancellationToken);
        return this.HandleResult(result);
    }
}

public sealed record CreateCartItemRequest(string Description, string? ItemId, decimal Price, double Quantity, decimal Total);