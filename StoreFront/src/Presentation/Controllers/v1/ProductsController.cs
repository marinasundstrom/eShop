using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;
using YourBrand.StoreFront.Application.Common.Models;
using YourBrand.StoreFront.Application.Products;

namespace YourBrand.StoreFront.Presentation.Controllers;

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
        string? itemGroupId = null, string? itemGroup2Id = null, string? itemGroup3Id = null,
        int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null,
        YourBrand.Catalog.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProducts(itemGroupId, itemGroup2Id, itemGroup3Id,
            page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<SiteProductDto?> GetProduct(string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProduct(id), cancellationToken);
    }

    [HttpGet("{id}/Variants")]
    public async Task<ItemsResult<SiteProductDto>> GetProductVariants(string id, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, YourBrand.Catalog.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProductVariants(id, page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpPost("{id}/Variants/Find")]
    public async Task<SiteProductDto?> FindProductVariantByAttributes(string id, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new FindProductVariantByAttributes(id, attributes), cancellationToken);
    }

    [HttpPost("{id}/Variants/Find2")]
    public async Task<IEnumerable<SiteProductDto>> FindProductVariantByAttributes2(string id, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {

        return await mediator.Send(new FindProductVariantByAttributes2(id, attributes), cancellationToken);
    }

    [HttpGet("Categories")]
    public async Task<IEnumerable<ProductGroupDto>?> GetProductGroups(string? parentGroupId, bool includeWithUnlisted = false, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetProductGroups(parentGroupId, includeWithUnlisted), cancellationToken);
    }
}
