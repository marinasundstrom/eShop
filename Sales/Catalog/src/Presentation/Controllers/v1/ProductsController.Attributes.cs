using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application.Products;
using YourBrand.Catalog.Application.Products.Attributes;
using YourBrand.Catalog.Application.Products.Variants;

namespace YourBrand.Catalog.Presentation.Controllers;

partial class ProductsController : Controller
{
    [HttpGet("{productId}/Attributes")]
    public async Task<ActionResult<IEnumerable<ProductAttributeDto>>> GetProductAttributes(string productId)
    {
        return Ok(await _mediator.Send(new GetProductAttributes(productId)));
    }

    [HttpPost("{productId}/Attributes")]
    public async Task<ActionResult<ProductAttributeDto>> AddProductAttribute(string productId, AddProductAttributeDto data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new AddProductAttribute(productId, data.AttributeId, data.ValueId), cancellationToken));
    }

    [HttpPut("{productId}/Attributes/{attributeId}")]
    public async Task<ActionResult<ProductAttributeDto>> UpdateProductAttribute(string productId, string attributeId, UpdateProductAttributeDto data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateProductAttribute(productId, attributeId, data.ValueId), cancellationToken));
    }

    [HttpDelete("{productId}/Attributes/{attributeId}")]
    public async Task<ActionResult> DeleteProductAttribute(string productId, string attributeId)
    {
        await _mediator.Send(new DeleteProductAttribute(productId, attributeId));
        return Ok();
    }

    [HttpPost("{productId}/Attributes/{attributeId}/GetAvailableValues")]
    public async Task<ActionResult<IEnumerable<Application.Attributes.AttributeValueDto>>> GetAvailableAttributeValues(string productId, string attributeId, Dictionary<string, string?> selectedAttributes)
    {
        return Ok(await _mediator.Send(new GetAvailableAttributeValues(productId, attributeId, selectedAttributes)));
    }
}

public sealed record AddProductAttributeDto(string AttributeId, string ValueId);

public sealed record UpdateProductAttributeDto(string ValueId);