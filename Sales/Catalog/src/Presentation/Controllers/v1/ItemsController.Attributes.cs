using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Application;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Products.Attributes;
using YourBrand.Catalog.Application.Products.Variants;

namespace YourBrand.Catalog.Presentation.Controllers;

partial class ProductsController : Controller
{
    [HttpGet("{productId}/Attributes")]
    public async Task<ActionResult<IEnumerable<AttributeDto>>> GetProductAttributes(string productId)
    {
        return Ok(await _mediator.Send(new GetProductAttributes(productId)));
    }

    [HttpPost("{productId}/Attributes")]
    public async Task<ActionResult<AttributeDto>> CreateProductAttribute(string productId, ApiAddProductAttribute data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new AddProductAttribute(productId, data), cancellationToken));
    }

    [HttpPut("{productId}/Attributes/{attributeId}")]
    public async Task<ActionResult<AttributeDto>> UpdateProductAttribute(string productId, string attributeId, ApiUpdateProductAttribute data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new UpdateProductAttribute(productId, attributeId, data), cancellationToken));
    }

    [HttpDelete("{productId}/Attributes/{attributeId}")]
    public async Task<ActionResult> DeleteProductAttribute(string productId, string attributeId)
    {
        await _mediator.Send(new DeleteProductAttribute(productId, attributeId));
        return Ok();
    }

    [HttpPost("{productId}/Attributes/{attributeId}/GetAvailableValues")]
    public async Task<ActionResult<IEnumerable<AttributeValueDto>>> GetAvailableAttributeValues(string productId, string attributeId, Dictionary<string, string?> selectedAttributes)
    {
        return Ok(await _mediator.Send(new GetAvailableAttributeValues(productId, attributeId, selectedAttributes)));
    }
}
