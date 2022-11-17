using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.CustomerService.Application;
using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Tickets.Commands;
using YourBrand.CustomerService.Application.Tickets.Dtos;
using YourBrand.CustomerService.Application.Tickets.Queries;

namespace YourBrand.CustomerService.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[Authorize]
public sealed class TicketsController : ControllerBase
{
    private readonly IMediator mediator;

    public TicketsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [EnableRateLimitingAttribute("MyControllerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<TicketDto>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<TicketDto>> GetTickets(TicketStatusDto? status, string? assigneeId, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetTickets(status, assigneeId, page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<TicketDto>> GetTicketById(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetTicketById(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TicketDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateTicket(request.Title, request.Description, request.Status, request.AssigneeId, request.EstimatedHours, request.RemainingHours), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetTicketById), new { id = data.Id }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteTicket(int id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteTicket(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/title")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateTitle(int id, [FromBody] string title, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateTitle(id, title), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/description")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateDescription(int id, [FromBody] string? description, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateDescription(id, description), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateStatus(int id, [FromBody] TicketStatusDto status, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateStatus(id, status), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/assignedUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateAssignedUser(int id, [FromBody] string? userId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateAssignedUser(id, userId), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/estimatedHours")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateEstimatedHours(int id, [FromBody] double? hours, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateEstimatedHours(id, hours), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPut("{id}/remainingHours")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateRemainingHours(int id, [FromBody] double? hours, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateRemainingHours(id, hours), cancellationToken);
        return this.HandleResult(result);
    }
}

public sealed record CreateTicketRequest(string Title, string? Description, TicketStatusDto Status, string? AssigneeId, double? EstimatedHours, double? RemainingHours);
