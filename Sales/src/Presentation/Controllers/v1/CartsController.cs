using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Sales.Application;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Carts.Commands;
using YourBrand.Sales.Application.Carts.Dtos;
using YourBrand.Sales.Application.Carts.Queries;

namespace YourBrand.Sales.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[Authorize]
public sealed partial class CartsController : ControllerBase
{
    private readonly IMediator mediator;

    public CartsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [EnableRateLimitingAttribute("MyControllerPolicy")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<CartDto>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<CartDto>> GetCarts(CartStatusDto? status, string? assignedTo, int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetCarts(page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CartDto>> GetCartById(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCartById(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CartDto>> CreateCart(CreateCartRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateCart(), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetCartById), new { id = data.Id }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteCart(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteCart(id), cancellationToken);
        return this.HandleResult(result);
    }
}

public sealed record CreateCartRequest();
