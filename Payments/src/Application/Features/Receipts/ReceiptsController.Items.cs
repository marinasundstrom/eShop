using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Payments.Application;
using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Features.Receipts.Items.Commands;
using YourBrand.Payments.Application.Features.Receipts.Dtos;
using YourBrand.Payments.Application.Features.Receipts.Queries;

namespace YourBrand.Payments.Application.Features.Receipts;

partial class ReceiptsController
{
    [HttpPost("{id}/Items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReceiptItemDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReceiptItemDto>> CreateReceiptItem(string id, CreateReceiptItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateReceiptItem(id, request.Description, request.ItemId, request.Unit, request.UnitPrice, request.VatRate, request.Quantity, request.Notes), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpPut("{id}/Items")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReceiptItemDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReceiptItemDto>> UpdateReceiptItem(string id, string itemId, UpdateReceiptItemRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateReceiptItem(id, itemId, request.Description, request.ItemId, request.Unit, request.UnitPrice, request.VatRate, request.Quantity, request.Notes), cancellationToken);
        return result.Handle(
            onSuccess: data => Ok(data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}/Items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteReceiptItem(string id, string itemId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteReceiptItem(id, itemId), cancellationToken);
        return this.HandleResult(result);
    }
}

public record UpdateReceiptItemRequest(string Description, string? ItemId, string? Unit, decimal UnitPrice, double Quantity, double VatRate, string? Notes);