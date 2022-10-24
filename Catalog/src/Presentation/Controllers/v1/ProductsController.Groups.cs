using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Options;
using YourBrand.Catalog.Application.Items.Options.Groups;

namespace YourBrand.Catalog.Presentation.Controllers;

partial class ItemsController : Controller
{

    [HttpGet("Groups")]
    public async Task<ActionResult<IEnumerable<ItemGroupDto>>> GetItemGroups(bool includeWithUnlistedItems = false)
    {
        return Ok(await _mediator.Send(new GetItemGroups(includeWithUnlistedItems)));
    }

    [HttpPost("Groups")]
    public async Task<ActionResult<ItemGroupDto>> CreateItemGroup(ApiCreateItemGroup data)
    {
        return Ok(await _mediator.Send(new CreateItemGroup(data.Name, data)));
    }

    [HttpPut("Groups/{itemGroupId}")]
    public async Task<ActionResult<ItemGroupDto>> UpdateItemGroup(string itemGroupId, ApiUpdateItemGroup data)
    {
        return Ok(await _mediator.Send(new UpdateItemGroup(itemGroupId, data)));
    }

    [HttpDelete("Groups/{itemGroupId}")]
    public async Task<ActionResult> DeleteItemGroup(string itemGroupId)
    {
        await  _mediator.Send(new DeleteItemGroup(itemGroupId));
        return Ok();
    }
}
