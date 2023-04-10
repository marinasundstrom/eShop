using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Common.Models;
using YourBrand.Catalog.Features.Products.Groups;
using YourBrand.Catalog.Features.Products.Import;

namespace YourBrand.Catalog.Features.Products;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public partial class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ItemsResult<ProductDto>>> GetProducts(
        string? storeId = null, bool includeUnlisted = false, bool groupProducts = true, string? productGroupIdOrPath = null,
        int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProducts(storeId, includeUnlisted, groupProducts, productGroupIdOrPath, page, pageSize, searchString, sortBy, sortDirection), cancellationToken));
    }

    [HttpGet("{productIdOrHandle}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string productIdOrHandle, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new GetProduct(productIdOrHandle), cancellationToken));
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult> UpdateProductDetails(long productId, ApiUpdateProductDetails details, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateProductDetails(productId, details), cancellationToken);
        return Ok();
    }

    [HttpPost("{productId}/UploadImage")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<ActionResult> UploadProductImage([FromRoute] long productId, IFormFile file, CancellationToken cancellationToken)
    {
        var url = await _mediator.Send(new UploadProductImage(productId, file.FileName, file.OpenReadStream()), cancellationToken);
        return Ok(url);
    }

    [HttpPost("{productId}/Visibility")]
    public async Task<ActionResult> UpdateProductVisibility(long productId, ProductVisibility visibility, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateProductVisibility(productId, visibility), cancellationToken);
        return Ok();
    }

    [HttpPost("{productId}/Group")]
    public async Task<ActionResult<ProductGroupDto>> UpdateProductGroup(long productId, long groupId, CancellationToken cancellationToken)
    {
        var dto = await _mediator.Send(new UpdateProductGroup(productId, groupId), cancellationToken);
        return Ok(dto);
    }

    [HttpPost("{productId}/Price")]
    public async Task<ActionResult> UpdateProductPrice(long productId, UpdateProductPriceRequest dto, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateProductPrice(productId, dto.Price), cancellationToken);
        return Ok();
    }

    [HttpPost("{productId}/Sku")]
    public async Task<ActionResult> UpdateProductSku(long productId, string sku, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateProductSku(productId, sku), cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(ApiCreateProduct data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CreateProduct(data.Name, data.Handle, data.StoreId, data.HasVariants, data.Description, data.BrandId, data.GroupId, data.Sku, data.Price, data.Visibility), cancellationToken));
    }

    [HttpPost("UploadProductImages")]
    public async Task<ActionResult> UploadProductImages(IFormFile file, CancellationToken cancellationToken)
    {   
        await _mediator.Send(new UploadProductImages(file.OpenReadStream()), cancellationToken);
        return Ok();
    }

    [HttpPost("ImportProductsCsv")]
    public async Task<ActionResult> ImportProductsCsv(IFormFile file, CancellationToken cancellationToken)
    {   
        var errors = await _mediator.Send(new ImportProductsCsv(file.OpenReadStream()), cancellationToken);
        return Ok(errors);
    }
}

public sealed record UpdateProductPriceRequest(decimal Price);