using Microsoft.AspNetCore.Mvc;
using Site.Shared;
using YourBrand.Catalog;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ILogger<ItemsController> _logger;
    private readonly YourBrand.Catalog.IItemsClient _itemsClient;
    private readonly IItemGroupsClient itemGroupsClient;
    private readonly YourBrand.Inventory.IItemsClient _inventoryItemsClient;

    public ItemsController(
        ILogger<ItemsController> logger, 
        YourBrand.Catalog.IItemsClient itemsClient,
        YourBrand.Catalog.IItemGroupsClient itemGroupsClient,
        YourBrand.Inventory.IItemsClient inventoryItemsClient)
    {
        _logger = logger;
        _itemsClient = itemsClient;
        this.itemGroupsClient = itemGroupsClient;
        _inventoryItemsClient = inventoryItemsClient;
    }

    [HttpGet]
    public async Task<ItemsResult<SiteItemDto>> GetItems(string? itemGroupId = null, string? itemGroup2Id = null, string? itemGroup3Id = null, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        var result = await _itemsClient.GetItemsAsync(false, true, itemGroupId, itemGroup2Id, itemGroup3Id, page - 1, pageSize, searchString, sortBy, sortDirection, cancellationToken);
        List<SiteItemDto> items = new List<SiteItemDto>();
        foreach(var item in result.Items) 
        {
            /*
            int? available = null;
            try 
            {
                var inventoryItem = await _inventoryItemsClient.GetItemAsync(item.Id, cancellationToken);
                available = inventoryItem.QuantityAvailable;
            } catch {}
            */

            items.Add(item.ToDto());
        }
        return new ItemsResult<SiteItemDto>(items, result.TotalItems);
    }

    [HttpGet("{id}")]
    public async Task<SiteItemDto?> GetItem(string id, CancellationToken cancellationToken = default)
    {
        var item = await _itemsClient.GetItemAsync(id, cancellationToken);
        /*
        int? available = null;
        try 
        {
            var inventoryItem = await _inventoryItemsClient.GetItemAsync(item.Id, cancellationToken);
            available = inventoryItem.QuantityAvailable;
        } catch {}
        */
        return item.ToDto();
    }

    [HttpGet("{id}/Attributes")]
    public async Task<IEnumerable<AttributeDto>> GetItemAttributes(string id, CancellationToken cancellationToken = default)
    {
        return await _itemsClient.GetItemAttributesAsync(id, cancellationToken);
    }

    [HttpGet("{id}/Options")]
    public async Task<IEnumerable<OptionDto>> GetItemOptions(string id, CancellationToken cancellationToken = default)
    {
        return await _itemsClient.GetItemOptionsAsync(id, null, cancellationToken);
    }

    [HttpGet("{id}/Variants")]
    public async Task<ItemsResultOfItemDto> GetItemVariants(string id, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _itemsClient.GetVariantsAsync(id, page - 1, pageSize, searchString, sortBy, sortDirection, cancellationToken);
    }

    [HttpPost("{id}/Variants/Find")]
    public async Task<SiteItemDto?> FindItemVariantByAttributes(string id, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {
        var r = await _itemsClient.FindVariantByAttributeValuesAsync(id, attributes, cancellationToken);
        return r?.ToDto();
    }

    [HttpPost("{id}/Variants/Find2")]
    public async Task<IEnumerable<SiteItemDto>> FindItemVariantByAttributes2(string id, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {
        var r = await _itemsClient.FindVariantByAttributeValues2Async(id, attributes, cancellationToken);
        return r.Select(x => x.ToDto());
    }

    [HttpGet("Categories")]
    public async Task<ICollection<ItemGroupDto>?> GetItemGroups(string? parentGroupId, bool includeWithUnlisted = false, CancellationToken cancellationToken = default)
    {
        return await itemGroupsClient.GetItemGroupsAsync(parentGroupId, includeWithUnlisted, false, cancellationToken);
    }
}
