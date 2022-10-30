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
    private readonly YourBrand.Inventory.IItemsClient _inventoryItemsClient;

    public ItemsController(
        ILogger<ItemsController> logger, 
        YourBrand.Catalog.IItemsClient itemsClient,
        YourBrand.Inventory.IItemsClient inventoryItemsClient)
    {
        _logger = logger;
        _itemsClient = itemsClient;
        _inventoryItemsClient = inventoryItemsClient;
    }

    [HttpGet]
    public async Task<ItemsResult<SiteItemDto>> GetItems(string? itemGroupId = null, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        var result = await _itemsClient.GetItemsAsync(false, true, itemGroupId, page - 1, pageSize, searchString, sortBy, sortDirection, cancellationToken);
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

            items.Add(new SiteItemDto(item.Id, item.Name, item.Description, new SiteItemGroupDto(item.Group.Id, item.Group.Name), item.Image, item.Price, item.CompareAtPrice, item.QuantityAvailable));
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
        return new SiteItemDto(item.Id, item.Name, item.Description, new SiteItemGroupDto(item.Group.Id, item.Group.Name), item.Image, item.Price, item.CompareAtPrice, item.QuantityAvailable);
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
    public async Task<ItemDto?> FindItemVariantByAttributes(string id, Dictionary<string, string> attributes, CancellationToken cancellationToken = default)
    {
        return await _itemsClient.FindVariantByAttributeValuesAsync(id, attributes, cancellationToken);
    }

    [HttpGet("Categories")]
    public async Task<ICollection<ItemGroupDto>?> GetItemGroups(string? parentGroupId, CancellationToken cancellationToken = default)
    {
        return await _itemsClient.GetItemGroupsAsync(parentGroupId, false, true, cancellationToken);
    }
}
