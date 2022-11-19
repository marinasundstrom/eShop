using System.Globalization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;
using YourBrand.StoreFront.Application.Common.Models;
using YourBrand.StoreFront.Application.Items;

namespace YourBrand.StoreFront.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
[ResponseCache(CacheProfileName = "Default30")]
public class ItemsController : ControllerBase
{
    private readonly ILogger<ItemsController> _logger;
    private readonly YourBrand.Catalog.IItemsClient _itemsClient;
    private readonly IItemGroupsClient itemGroupsClient;
    private readonly YourBrand.Inventory.IItemsClient _inventoryItemsClient;
    private readonly IMediator mediator;

    public ItemsController(
        ILogger<ItemsController> logger,
        YourBrand.Catalog.IItemsClient itemsClient,
        YourBrand.Catalog.IItemGroupsClient itemGroupsClient,
        YourBrand.Inventory.IItemsClient inventoryItemsClient,
        IMediator mediator)
    {
        _logger = logger;
        _itemsClient = itemsClient;
        this.itemGroupsClient = itemGroupsClient;
        _inventoryItemsClient = inventoryItemsClient;
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ItemsResult<SiteItemDto>> GetItems(
        string? itemGroupId = null, string? itemGroup2Id = null, string? itemGroup3Id = null,
        int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null,
        YourBrand.Catalog.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetItems(itemGroupId, itemGroup2Id, itemGroup3Id,
            page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<SiteItemDto?> GetItem(string id, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetItem(id), cancellationToken);
    }

    [HttpGet("{id}/Variants")]
    public async Task<ItemsResult<SiteItemDto>> GetItemVariants(string id, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, YourBrand.Catalog.SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetItemVariants(id, page, pageSize, searchString, sortBy, sortDirection), cancellationToken);
    }

    [HttpPost("{id}/Variants/Find")]
    public async Task<SiteItemDto?> FindItemVariantByAttributes(string id, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new FindItemVariantByAttributes(id, attributes), cancellationToken);
    }

    [HttpPost("{id}/Variants/Find2")]
    public async Task<IEnumerable<SiteItemDto>> FindItemVariantByAttributes2(string id, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {

        return await mediator.Send(new FindItemVariantByAttributes2(id, attributes), cancellationToken);
    }

    [HttpGet("Categories")]
    public async Task<IEnumerable<ItemGroupDto>?> GetItemGroups(string? parentGroupId, bool includeWithUnlisted = false, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetItemGroups(parentGroupId, includeWithUnlisted), cancellationToken);
    }
}
