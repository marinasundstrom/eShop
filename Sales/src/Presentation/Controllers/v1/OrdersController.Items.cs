using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Sales.Application;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Orders.Items.Commands;
using YourBrand.Sales.Application.Orders.Dtos;
using YourBrand.Sales.Application.Orders.Queries;

namespace YourBrand.Sales.Presentation.Controllers;

partial class OrdersController
{
    [HttpPost("{id}/Items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderItemDto>> CreateOrderItem(int id, CreateOrderItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateOrderItem(id, request.Description, request.ItemId, request.Unit, request.UnitPrice, request.VatRate, request.Quantity), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpPut("{id}/Items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderItemDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderItemDto>> UpdateOrderItem(int id, string itemId, UpdateOrderItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateOrderItem(id, itemId, request.Description, request.ItemId, request.Unit, request.UnitPrice, request.VatRate, request.Quantity), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}/Items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrderItem(int id, string itemId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteOrderItem(id, itemId), cancellationToken);
        return this.HandleResult(result);
    }
}

public record UpdateOrderItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate);