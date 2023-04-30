using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Features.Carts;
using YourBrand.StoreFront.Application.Common.Models;

namespace YourBrand.StoreFront.Application.Features.Products;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[ResponseCache(CacheProfileName = "Default30")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IMediator mediator;

    public ProductsController(
        ILogger<ProductsController> logger,
        IMediator mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ItemsResult<SiteProductDto>> GetProducts(
        string? productGroupIdOrPath = null, string? brandIdOrHandle = null,
        int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null,
        YourBrand.Catalog.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProducts(productGroupIdOrPath, brandIdOrHandle,
            page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{productIdOrHandle}")]
    public async Task<SiteProductDto?> GetProduct(string productIdOrHandle, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProduct(productIdOrHandle), cancellationToken);
    }

    [HttpGet("{productIdOrHandle}/Variants")]
    public async Task<ItemsResult<SiteProductDto>> GetProductVariants(string productIdOrHandle, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, YourBrand.Catalog.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProductVariants(productIdOrHandle, page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpPost("{productIdOrHandle}/Variants/Find")]
    public async Task<SiteProductDto?> FindProductVariantByAttributes(string productIdOrHandle, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new FindProductVariantByAttributes(productIdOrHandle, attributes), cancellationToken);
    }

    [HttpPost("{productIdOrHandle}/Variants/Find2")]
    public async Task<IEnumerable<SiteProductDto>> FindProductVariantByAttributes2(string productIdOrHandle, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {

        return await mediator.Send(new FindProductVariantByAttributes2(productIdOrHandle, attributes), cancellationToken);
    }

    [HttpGet("Categories")]
    public async Task<IEnumerable<ProductGroupDto>?> GetProductGroups(long? parentGroupId, bool includeWithUnlisted = false, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProductGroups(parentGroupId, includeWithUnlisted), cancellationToken);
    }

    [HttpGet("Categories/{*productGroupIdOrPath}")]
    public async Task<ProductGroupDto?> GetProductGroup(string productGroupIdOrPath, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProductGroup(productGroupIdOrPath), cancellationToken);
    }
}
