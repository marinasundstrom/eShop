using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Pricing.Application;
using YourBrand.Pricing.Application.Common;
using YourBrand.Pricing.Application.Orders.Commands;
using YourBrand.Pricing.Application.Orders.Dtos;
using YourBrand.Pricing.Application.Orders.Queries;

namespace YourBrand.Pricing.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public sealed partial class OrdersController : ControllerBase
{
    private readonly IMediator mediator;

    public OrdersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<OrderDto>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<OrderDto>> GetOrders([FromQueryAttribute] int[]? status, string? customerId, string? ssn, string? assigneeId, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetOrders(status, customerId, ssn, assigneeId, page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderDto>> GetOrderById(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderById(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpGet("GetOrderByNo/{orderNo}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderDto>> GetOrderByNo(int orderNo, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderByNo(orderNo), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateOrder(request.CustomerId, request.BillingDetails, request.ShippingDetails, request.Items), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetOrderById), new { id = data.OrderNo }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpPost("Draft")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderDto>> CreateDraftOrder(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateDraftOrder(), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetOrderById), new { id = data.OrderNo }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrder(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteOrder(id), cancellationToken);
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
