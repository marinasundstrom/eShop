using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Sales.Application;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Orders.Commands;
using YourBrand.Sales.Application.Orders.Dtos;
using YourBrand.Sales.Application.Orders.Queries;

namespace YourBrand.Sales.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[Authorize]
public sealed partial class OrdersController : ControllerBase
{
    private readonly IMediator mediator;

    public OrdersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [EnableRateLimitingAttribute("MyControllerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<OrderDto>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<OrderDto>> GetOrders(OrderStatusDto? status, string? assignedTo, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetOrders(status, assignedTo, page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderDto>> GetOrderById(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetOrderById(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateOrder(request.Title, request.Description, request.Status, request.AssignedTo, request.EstimatedHours, request.RemainingHours), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetOrderById), new { id = data.Id }, data),
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
    public async Task<ActionResult> UpdateStatus(string id, [FromBody] OrderStatusDto status, CancellationToken cancellationToken)
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
