using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Products.Groups;
using YourBrand.Catalog.Application.Products.Options;
using YourBrand.Catalog.Application.Products.Options.Groups;
using MediatR;

namespace YourBrand.Catalog.Presentation.Controllers;

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
    public async Task<ActionResult<IEnumerable<ProductGroupDto>>> GetProductGroups(string? storeId = null, string? parentGroupId = null, bool includeWithUnlistedProducts = false, bool IncludeHidden = false, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProductGroups(storeId, parentGroupId, includeWithUnlistedProducts, IncludeHidden), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductGroupDto>> GetProductGroup(string id, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProductGroup(id), cancellationToken));
    }
    [HttpPost]
    public async Task<ActionResult<ProductGroupDto>> CreateProductGroup(ApiCreateProductGroup data, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new CreateProductGroup(data.Name, data), cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductGroupDto>> UpdateProductGroup(string id, ApiUpdateProductGroup data, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new UpdateProductGroup(id, data), cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductGroup(string id, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new DeleteProductGroup(id), cancellationToken);
        return Ok();
    }
}
