using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Options;
using YourBrand.Catalog.Application.Items.Options.Groups;
using MediatR;

namespace YourBrand.Catalog.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public partial class ItemGroupsController : Controller
{
    private readonly IMediator _mediator;

    public ItemGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemGroupDto>>> GetItemGroups(string? parentGroupId = null, bool includeWithUnlistedItems = false, bool IncludeHidden = false, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetItemGroups(parentGroupId, includeWithUnlistedItems, IncludeHidden), cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemGroupDto>> GetItemGroup(string id, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetItemGroup(id), cancellationToken));
    }
    [HttpPost]
    public async Task<ActionResult<ItemGroupDto>> CreateItemGroup(ApiCreateItemGroup data, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new CreateItemGroup(data.Name, data), cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemGroupDto>> UpdateItemGroup(string id, ApiUpdateItemGroup data, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new UpdateItemGroup(id, data), cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemGroup(string id, CancellationToken cancellationToken = default)
    {
        await  _mediator.Send(new DeleteItemGroup(id), cancellationToken);
        return Ok();
    }
}
