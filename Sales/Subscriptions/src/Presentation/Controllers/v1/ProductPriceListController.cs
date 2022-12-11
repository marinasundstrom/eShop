using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using YourBrand.Subscriptions.Application;
using YourBrand.Subscriptions.Application.Common;
using YourBrand.Subscriptions.Application.ProductPriceLists.Commands;
using YourBrand.Subscriptions.Application.ProductPriceLists.Dtos;
using YourBrand.Subscriptions.Application.ProductPriceLists.Queries;

namespace YourBrand.Subscriptions.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public sealed partial class ProductPriceListsController : ControllerBase
{
    private readonly IMediator mediator;

    public ProductPriceListsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ItemsResult<ProductPriceListDto>))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesDefaultResponseType]
    public async Task<ItemsResult<ProductPriceListDto>> GetProductPriceLists(int page = 1, int pageSize = 10, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
        => await mediator.Send(new GetProductPriceLists(page, pageSize, sortBy, sortDirection), cancellationToken);

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductPriceListDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductPriceListDto>> GetProductPriceListById(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductPriceListById(id), cancellationToken);
        return this.HandleResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductPriceListDto))]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ProductPriceListDto>> CreateProductPriceList(CreateProductPriceListRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateProductPriceList(request.Name), cancellationToken);
        return result.Handle(
            onSuccess: data => CreatedAtAction(nameof(GetProductPriceListById), new { id = data.Id }, data),
            onError: error => Problem(detail: error.Detail, title: error.Title, type: error.Id));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteProductPriceList(string id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeleteProductPriceList(id), cancellationToken);
        return this.HandleResult(result);
    }
}
