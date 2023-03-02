using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Features.Products.Attributes;
using YourBrand.Catalog.Features.Products.Variants;

namespace YourBrand.Catalog.Features.Products;

partial class ProductsController : Controller
{
    [HttpGet("{productId}/Attributes")]
    public async Task<ActionResult<IEnumerable<ProductAttributeDto>>> GetProductAttributes(long productId)
    {
        return Ok(await _mediator.Send(new GetProductAttributes(productId)));
    }

    [HttpPost("{productId}/Attributes")]
    public async Task<ActionResult<ProductAttributeDto>> AddProductAttribute(long productId, AddProductAttributeDto data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new AddProductAttribute(productId, data.AttributeId, data.ValueId), cancellationToken));
    }

    [HttpPut("{productId}/Attributes/{attributeId}")]
    public async Task<ActionResult<ProductAttributeDto>> UpdateProductAttribute(long productId, string attributeId, UpdateProductAttributeDto data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateProductAttribute(productId, attributeId, data.ValueId), cancellationToken));
    }

    [HttpDelete("{productId}/Attributes/{attributeId}")]
    public async Task<ActionResult> DeleteProductAttribute(long productId, string attributeId)
    {
        await _mediator.Send(new DeleteProductAttribute(productId, attributeId));
        return Ok();
    }

    [HttpPost("{productId}/Attributes/{attributeId}/GetAvailableValues")]
    public async Task<ActionResult<IEnumerable<Features.Attributes.AttributeValueDto>>> GetAvailableAttributeValues(long productId, string attributeId, Dictionary<string, string?> selectedAttributes)
    {
        return Ok(await _mediator.Send(new GetAvailableAttributeValues(productId, attributeId, selectedAttributes)));
    }
}

public sealed record AddProductAttributeDto(string AttributeId, string ValueId);

public sealed record UpdateProductAttributeDto(string ValueId);