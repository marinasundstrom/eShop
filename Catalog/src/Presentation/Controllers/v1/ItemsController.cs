using MediatR;

using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Application.Items;
using Microsoft.AspNetCore.Http;

namespace YourBrand.Catalog.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public partial class ItemsController : Controller
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ItemsResult<ItemDto>>> GetItems(
        string? storeId = null, bool includeUnlisted = false, bool groupItems = true, string? groupId = null, string? group2Id = null, string? group3Id = null,
        int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetItems(storeId, includeUnlisted, groupItems, groupId, group2Id, group3Id, page, pageSize, searchString, sortBy, sortDirection), cancellationToken));
    }

    [HttpGet("{itemId}")]
    public async Task<ActionResult<ItemDto>> GetItem(string itemId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetItem(itemId), cancellationToken));
    }

    [HttpPut("{itemId}")]
    public async Task<ActionResult> UpdateItemDetails(string itemId, ApiUpdateItemDetails details, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateItemDetails(itemId, details), cancellationToken);
        return Ok();
    }

    [HttpPost("{itemId}/UploadImage")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadItemImage([FromRoute] string itemId, IFormFile file, CancellationToken cancellationToken)
    {
        var url = await _mediator.Send(new UploadItemImage(itemId, file.FileName, file.OpenReadStream()), cancellationToken);
        return Ok(url);
    }

    [HttpGet("{itemId}/Visibility")]
    public async Task<ActionResult> UpdateItemVisibility(string itemId, ItemVisibility visibility, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateItemVisibility(itemId, visibility), cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(ApiCreateItem data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CreateItem(data.Id, data.Name, data.HasVariants, data.Description, data.GroupId, data.Price, data.Visibility), cancellationToken));
    }
}
