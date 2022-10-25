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

    public ItemsController(ILogger<ItemsController> logger, YourBrand.Catalog.IItemsClient itemsClient)
    {
        _logger = logger;
        _itemsClient = itemsClient;
    }

    [HttpGet]
    public async Task<ItemsResultOfItemDto> GetItems(string? itemGroupId = null, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _itemsClient.GetItemsAsync(false, true, itemGroupId, page - 1, pageSize, searchString, sortBy, sortDirection, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<ItemDto?> GetItem(string id, CancellationToken cancellationToken = default)
    {
        return await _itemsClient.GetItemAsync(id, cancellationToken);
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
    public async Task<ICollection<ItemGroupDto>?> GetItemGroups(CancellationToken cancellationToken = default)
    {
        return await _itemsClient.GetItemGroupsAsync(false, cancellationToken);
    }
}
