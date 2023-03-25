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

    [HttpGet("Tree")]
    public async Task<ActionResult<IEnumerable<ProductGroupTreeNodeDto>>> GetProductGroupsTree(string? storeId = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProductGroupTree(storeId), cancellationToken));
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
    public async Task<ActionResult<ProductGroupDto>> UpdateDetails(long id, ApiUpdateProductGroup data, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new UpdateDetails(id, data), cancellationToken));
    }

    [HttpPut("{id}/Parent")]
    public async Task<ActionResult<ProductGroupDto>> UpdateParent(long id, long? parent, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new UpdateParent(id, parent), cancellationToken));
    }

    [HttpPut("{id}/AllowItems")]
    public async Task<ActionResult<ProductGroupDto>> UpdateAllowItems(long id, bool allowItems, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new UpdateAllowItems(id, allowItems), cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductGroup(long id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteProductGroup(id), cancellationToken);
        return Ok();
    }
}