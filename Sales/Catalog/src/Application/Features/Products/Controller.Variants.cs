using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Common.Models;
using YourBrand.Catalog.Features.Products.Variants;

namespace YourBrand.Catalog.Features.Products;

partial class ProductsController : Controller
{
    [HttpGet("{productIdOrHandle}/Variants")]
    public async Task<ActionResult<ItemsResult<ProductDto>>> GetVariants(string productIdOrHandle, int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProductVariants(productIdOrHandle, page, pageSize, searchString, sortBy, sortDirection)));
    }

    [HttpDelete("{productId}/Variants/{variantId}")]
    public async Task<ActionResult> DeleteVariant(long productId, long variantId)
    {
        await _mediator.Send(new DeleteProductVariant(productId, variantId));
        return Ok();
    }

    [HttpGet("{productIdOrHandle}/Variants/{variantIdOrHandle}")]
    public async Task<ActionResult<ProductDto>> GetVariant(string productIdOrHandle, string variantIdOrHandle)
    {
        return Ok(await _mediator.Send(new GetProductVariant(productIdOrHandle, variantIdOrHandle)));
    }

    [HttpPost("{productIdOrHandle}/Variants/Find")]
    public async Task<ActionResult<ProductDto>> FindVariantByAttributeValues(string productIdOrHandle, Dictionary<string, string?> selectedAttributeValues)
    {
        return Ok(await _mediator.Send(new FindProductVariant(productIdOrHandle, selectedAttributeValues)));
    }

    [HttpPost("{productIdOrHandle}/Variants/Find2")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> FindVariantByAttributeValues2(string productIdOrHandle, Dictionary<string, string?> selectedAttributeValues)
    {
        return Ok(await _mediator.Send(new FindProductVariants(productIdOrHandle, selectedAttributeValues)));
    }

    [HttpGet("{productId}/Variants/{variantId}/Options")]
    public async Task<ActionResult<ProductVariantAttributeDto>> GetVariantAttributes(long productId, long variantId)
    {
        return Ok(await _mediator.Send(new GetProductVariantAttributes(productId, variantId)));
    }

    [HttpPost("{productId}/Variants")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> CreateVariant(long productId, ApiCreateProductVariant data)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateProductVariant(productId, data)));
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

    [HttpPut("{productId}/Variants/{variantId}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductDto>> UpdateVariant(long productId, long variantId, ApiUpdateProductVariant data)
    {
        try
        {
            return Ok(await _mediator.Send(new UpdateProductVariant(productId, variantId, data)));
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

    [HttpPost("{productId}/Variants/{variantId}/UploadImage")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadVariantImage([FromRoute] long productId, long variantId, IFormFile file, CancellationToken cancellationToken)
    {
        var url = await _mediator.Send(new UploadProductVariantImage(productId, variantId, file.Name, file.OpenReadStream()), cancellationToken);
        return Ok(url);
    }
}