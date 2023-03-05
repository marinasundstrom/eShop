using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using YourBrand.Catalog.Common.Models;

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
        string? storeId = null, bool includeUnlisted = false, bool groupProducts = true, string? groupId = null, string? group2Id = null, string? group3Id = null,
        int page = 0, int pageSize = 10, string? searchString = null, string? sortBy = null, Common.Models.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(new GetProducts(storeId, includeUnlisted, groupProducts, groupId, group2Id, group3Id, page, pageSize, searchString, sortBy, sortDirection), cancellationToken));
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

    [HttpGet("{productId}/Visibility")]
    public async Task<ActionResult> UpdateProductVisibility(long productId, ProductVisibility visibility, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateProductVisibility(productId, visibility), cancellationToken);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct(ApiCreateProduct data, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(new CreateProduct(data.Name, data.Handle, data.StoreId, data.HasVariants, data.Description, data.GroupId, data.Sku, data.Price, data.Visibility), cancellationToken));
    }
}