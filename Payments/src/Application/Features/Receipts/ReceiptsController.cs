using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Payments.Application;
using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Features.Receipts.Commands;
using YourBrand.Payments.Application.Features.Receipts.Dtos;
using YourBrand.Payments.Application.Features.Receipts.Queries;

namespace YourBrand.Payments.Application.Features.Receipts;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public sealed partial class ReceiptsController : ControllerBase
{
    private readonly IMediator mediator;

    public ReceiptsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<ReceiptDto>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<ReceiptDto>> GetReceipts([FromQueryAttribute] int[]? status, string? customerId, string? ssn, string? assigneeId, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetReceipts(status, customerId, ssn, assigneeId, page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReceiptDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReceiptDto>> GetReceiptById(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetReceiptById(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpGet("GetReceiptByNo/{receiptNo}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReceiptDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReceiptDto>> GetReceiptByNo(int receiptNo, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetReceiptByNo(receiptNo), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReceiptDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReceiptDto>> CreateReceipt(CreateReceiptRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateReceipt(request.CustomerId, request.BillingDetails, request.ShippingDetails, request.Items), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetReceiptById), new { id = data.ReceiptNo }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpPost("Draft")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReceiptDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReceiptDto>> CreateDraftReceipt(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateDraftReceipt(), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetReceiptById), new { id = data.ReceiptNo }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteReceipt(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteReceipt(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/Status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateStatus(string id, [FromBody] int status, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateStatus(id, status), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/AssignedUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateAssignedUser(string id, [FromBody] string? userId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateAssignedUser(id, userId), cancellationToken);
        return this.HandleResult(result);
    }
}
