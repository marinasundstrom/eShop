using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;
using YourBrand.StoreFront.Application.Common.Models;

namespace YourBrand.StoreFront.Application.Items;

public sealed record GetItemVariants(
    string Id, int Page = 1, int PageSize = 10,
    string? SearchString = null, string? SortBy = null, YourBrand.Catalog.SortDirection? SortDirection = null)
    : IRequest<ItemsResult<SiteItemDto>>
{
    sealed class Handler : IRequestHandler<GetItemVariants, ItemsResult<SiteItemDto>>
    {
        private readonly YourBrand.Catalog.IItemsClient _itemsClient;
        private readonly IItemGroupsClient itemGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryItemsClient;

        public Handler(
            YourBrand.Catalog.IItemsClient itemsClient,
            YourBrand.Catalog.IItemGroupsClient itemGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryItemsClient)
        {

            _itemsClient = itemsClient;
            this.itemGroupsClient = itemGroupsClient;
            _inventoryItemsClient = inventoryItemsClient;
        }

        public async Task<ItemsResult<SiteItemDto>> Handle(GetItemVariants request, CancellationToken cancellationToken)
        {
            var result = await _itemsClient.GetVariantsAsync(request.Id, request.Page - 1, request.PageSize, request.SearchString, request.SortBy, request.SortDirection, cancellationToken);

            return new ItemsResult<SiteItemDto>(result.Items.Select(x => x.ToDto()), result.TotalItems);
        }
    }
}
