using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Items.Options;
using YourBrand.Catalog.Application.Items.Options.Groups;
using YourBrand.Catalog.Application.Items.Variants;

namespace YourBrand.Catalog.Presentation.Controllers;

partial class ItemsController : Controller
{
    [HttpGet("{itemId}/Options")]
    public async Task<ActionResult<IEnumerable<OptionDto>>> GetItemOptions(string itemId, string? variantId, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetItemOptions(itemId, variantId), cancellationToken));
    }

    [HttpPost("{itemId}/Options")]
    public async Task<ActionResult<OptionDto>> CreateItemOption(string itemId, ApiCreateItemOption data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CreateItemOption(itemId, data), cancellationToken));
    }

    [HttpPut("{itemId}/Options/{optionId}")]
    public async Task<ActionResult<OptionDto>> UpdateItemOption(string itemId, string optionId, ApiUpdateItemOption data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateItemOption(itemId, optionId, data), cancellationToken));
    }

    [HttpDelete("{itemId}/Options/{optionId}")]
    public async Task<ActionResult> DeleteItemOption(string itemId, string optionId)
    {
        await _mediator.Send(new DeleteItemOption(itemId, optionId));
        return Ok();
    }

    [HttpPost("{itemId}/Options/{optionId}/Values")]
    public async Task<ActionResult<OptionValueDto>> CreateItemOptionValue(string itemId, string optionId, ApiCreateItemOptionValue data, CancellationToken cancellationToken)
    {

        return Ok(await _mediator.Send(new CreateItemOptionValue(itemId, optionId, data), cancellationToken));
    }

    [HttpPost("{itemId}/Options/{optionId}/Values/{valueId}")]
    public async Task<ActionResult> DeleteItemOptionValue(string itemId, string optionId, string valueId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteItemOptionValue(itemId, optionId, valueId), cancellationToken);
        return Ok();
    }

    [HttpGet("{itemId}/Options/{optionId}/Values")]
    public async Task<ActionResult<IEnumerable<OptionValueDto>>> GetItemOptionValues(string itemId, string optionId)
    {
        return Ok(await _mediator.Send(new GetOptionValues(optionId)));
    }

    [HttpGet("{itemId}/Options/Groups")]
    public async Task<ActionResult<IEnumerable<OptionGroupDto>>> GetOptionGroups(string itemId)
    {
        return Ok(await _mediator.Send(new GetItemOptionGroups(itemId)));
    }

    [HttpPost("{itemId}/Options/Groups")]
    public async Task<ActionResult<OptionGroupDto>> CreateOptionGroup(string itemId, ApiCreateItemOptionGroup data)
    {
        return Ok(await _mediator.Send(new CreateItemOptionGroup(itemId, data)));
    }

    [HttpPut("{itemId}/Options/Groups/{optionGroupId}")]
    public async Task<ActionResult<OptionGroupDto>> UpdateOptionGroup(string itemId, string optionGroupId, ApiUpdateItemOptionGroup data)
    {
        return Ok(await _mediator.Send(new UpdateItemOptionGroup(itemId, optionGroupId, data)));
    }

    [HttpDelete("{itemId}/Options/Groups/{optionGroupId}")]
    public async Task<ActionResult> DeleteOptionGroup(string itemId, string optionGroupId)
    {
        await _mediator.Send(new DeleteItemOptionGroup(itemId, optionGroupId));
        return Ok();
    }
}
