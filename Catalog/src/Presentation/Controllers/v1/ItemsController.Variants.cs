using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Application.Items.Variants;
using Microsoft.AspNetCore.Http;
using YourBrand.Catalog.Application.Items;

namespace YourBrand.Catalog.Presentation.Controllers;

partial class ItemsController : Controller
{
    [HttpGet("{itemId}/Variants")]
    public async Task<ActionResult<ItemsResult<ItemDto>>> GetVariants(string itemId, int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, Application.Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetItemVariants(itemId, page, pageSize, searchString, sortBy, sortDirection)));
    }

    [HttpDelete("{itemId}/Variants/{variantId}")]
    public async Task<ActionResult> DeleteVariant(string itemId, string variantId)
    {
        await _mediator.Send(new DeleteItemVariant(itemId, variantId));
        return Ok();
    }

    [HttpGet("{itemId}/Variants/{variantId}")]
    public async Task<ActionResult<ItemDto>> GetVariant(string itemId, string variantId)
    {
        return Ok( await _mediator.Send(new GetItemVariant(itemId, variantId)));
    }

    [HttpPost("{itemId}/Variants/Find")]
    public async Task<ActionResult<ItemDto>> FindVariantByAttributeValues(string itemId, Dictionary<string, string?> selectedAttributeValues)
    {
        return Ok(await _mediator.Send(new FindItemVariant(itemId, selectedAttributeValues)));
    }

    [HttpPost("{itemId}/Variants/Find2")]
    public async Task<ActionResult<IEnumerable<ItemDto>>> FindVariantByAttributeValues2(string itemId, Dictionary<string, string?> selectedAttributeValues)
    {
        return Ok(await _mediator.Send(new FindItemVariants(itemId, selectedAttributeValues)));
    }

    [HttpGet("{itemId}/Variants/{variantId}/Options")]
    public async Task<ActionResult<ItemVariantAttributeDto>> GetVariantAttributes(string itemId, string variantId)
    {
        return Ok(await _mediator.Send(new GetItemVariantAttributes(itemId, variantId)));
    }

    [HttpPost("{itemId}/Variants")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ItemDto>> CreateVariant(string itemId, ApiCreateItemVariant data)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateItemVariant(itemId, data)));
        }
        catch (VariantAlreadyExistsException e)
        {
            return Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }

    [HttpPut("{itemId}/Variants/{variantId}")]
    [ProducesResponseType(typeof(ItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ItemDto>> UpdateVariant(string itemId, string variantId, ApiUpdateItemVariant data)
    {
        try
        {
            return Ok(await _mediator.Send(new UpdateItemVariant(itemId, variantId, data)));
        }
        catch (VariantAlreadyExistsException e)
        {
            return Problem(
                title: "Variant already exists.",
                detail: "There is already a variant with the chosen options.",
                instance: Request.Path,
                statusCode: StatusCodes.Status400BadRequest);
        }
    }
    
    [HttpPost("{itemId}/Variants/{variantId}/UploadImage")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadVariantImage([FromRoute] string itemId, string variantId, IFormFile file, CancellationToken cancellationToken)
    {
        var url = await _mediator.Send(new UploadItemVariantImage(itemId, variantId, file.Name, file.OpenReadStream()), cancellationToken); 
        return Ok(url);
    }
}