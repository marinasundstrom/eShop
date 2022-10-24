using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Items.Attributes;
using YourBrand.Catalog.Application.Items.Attributes.Groups;
using YourBrand.Catalog.Application.Items.Variants;

namespace YourBrand.Catalog.Presentation.Controllers;

partial class ItemsController : Controller
{
    [HttpGet("{itemId}/Attributes")]
    public async Task<ActionResult<IEnumerable<AttributeDto>>> GetItemAttributes(string itemId)
    {
        return Ok(await _mediator.Send(new GetItemAttributes(itemId)));
    }

    [HttpPost("{itemId}/Attributes")]
    public async Task<ActionResult<AttributeDto>> CreateItemAttribute(string itemId, ApiCreateItemAttribute data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CreateItemAttribute(itemId, data), cancellationToken));
    }

    [HttpPut("{itemId}/Attributes/{attributeId}")]
    public async Task<ActionResult<AttributeDto>> UpdateItemAttribute(string itemId, string attributeId, ApiUpdateItemAttribute data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateItemAttribute(itemId, attributeId, data), cancellationToken));
    }

    [HttpDelete("{itemId}/Attributes/{attributeId}")]
    public async Task<ActionResult> DeleteItemAttribute(string itemId, string attributeId)
    {
        await _mediator.Send(new DeleteItemAttribute(itemId, attributeId));
        return Ok();
    }

    [HttpPost("{itemId}/Attributes/{attributeId}/Values")]
    public async Task<ActionResult<AttributeValueDto>> CreateItemAttributeValue(string itemId, string attributeId, ApiCreateItemAttributeValue data, CancellationToken cancellationToken)
    {

        return Ok(await _mediator.Send(new CreateItemAttributeValue(itemId, attributeId, data), cancellationToken));
    }

    [HttpPost("{itemId}/Attributes/{attributeId}/Values/{valueId}")]
    public async Task<ActionResult> DeleteItemAttributeValue(string itemId, string attributeId, string valueId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteItemAttributeValue(itemId, attributeId, valueId), cancellationToken);
        return Ok();
    }

    [HttpGet("{itemId}/Attributes/{attributeId}/Values")]
    public async Task<ActionResult<IEnumerable<AttributeValueDto>>> GetItemAttributeValues(string itemId, string attributeId)
    {
        return Ok(await _mediator.Send(new GetAttributeValues(attributeId)));
    }

    [HttpGet("{itemId}/Attributes/Groups")]
    public async Task<ActionResult<IEnumerable<AttributeGroupDto>>> GetAttributeGroups(string itemId)
    {
        return Ok(await _mediator.Send(new GetItemAttributeGroups(itemId)));
    }

    [HttpPost("{itemId}/Attributes/Groups")]
    public async Task<ActionResult<AttributeGroupDto>> CreateAttributeGroup(string itemId, ApiCreateItemAttributeGroup data)
    {
        return Ok(await _mediator.Send(new CreateItemAttributeGroup(itemId, data)));
    }

    [HttpPut("{itemId}/Attributes/Groups/{attributeGroupId}")]
    public async Task<ActionResult<AttributeGroupDto>> UpdateAttributeGroup(string itemId, string attributeGroupId, ApiUpdateItemAttributeGroup data)
    {
        return Ok(await _mediator.Send(new UpdateItemAttributeGroup(itemId, attributeGroupId, data)));
    }

    [HttpDelete("{itemId}/Attributes/Groups/{attributeGroupId}")]
    public async Task<ActionResult> DeleteAttributeGroup(string itemId, string attributeGroupId)
    {
        await _mediator.Send(new DeleteItemAttributeGroup(itemId, attributeGroupId));
        return Ok();
    }

    [HttpPost("{itemId}/Attributes/{attributeId}/GetAvailableValues")]
    public async Task<ActionResult<IEnumerable<AttributeValueDto>>> GetAvailableAttributeValues(string itemId, string attributeId, Dictionary<string, string?> selectedAttributes)
    {
        return Ok(await _mediator.Send(new GetAvailableAttributeValues(itemId, attributeId, selectedAttributes)));
    }
}
