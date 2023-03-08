using MediatR;

using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Features.Products.Groups;

namespace YourBrand.Catalog.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public partial class ProductGroupsController : Controller
{
    private readonly IMediator _mediator;

    public ProductGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductGroupDto>>> GetProductGroups(string? storeId = null, long? parentGroupId = null, bool includeWithUnlistedProducts = false, bool IncludeHidden = false, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProductGroups(storeId, parentGroupId, includeWithUnlistedProducts, IncludeHidden), cancellationToken));
    }

    [HttpGet("{*productGroupIdOrPath}")]
    public async Task<ActionResult<ProductGroupDto>> GetProductGroup(string productGroupIdOrPath, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProductGroup(productGroupIdOrPath), cancellationToken));
    }
    [HttpPost]
    public async Task<ActionResult<ProductGroupDto>> CreateProductGroup(ApiCreateProductGroup data, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new CreateProductGroup(data), cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductGroupDto>> UpdateProductGroup(long id, ApiUpdateProductGroup data, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new UpdateProductGroup(id, data), cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductGroup(long id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteProductGroup(id), cancellationToken);
        return Ok();
    }
}