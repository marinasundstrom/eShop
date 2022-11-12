using System;
using System.Globalization;
using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;
using YourBrand.StoreFront.Application.Common.Models;
using YourBrand.StoreFront.Application.Services;

namespace YourBrand.StoreFront.Application.Items;

public sealed record GetItems(
    string? ItemGroupId = null,
    string? ItemGroup2Id = null,
    string? ItemGroup3Id = null,
    int Page = 1,
    int PageSize = 10,
    string? SearchString = null,
    string? SortBy = null,
    YourBrand.Catalog.SortDirection? SortDirection = null)
    : IRequest<ItemsResult<SiteItemDto>>
{
    sealed class Handler : IRequestHandler<GetItems, ItemsResult<SiteItemDto>>
    {
        private readonly YourBrand.Catalog.IItemsClient _itemsClient;
        private readonly IItemGroupsClient itemGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryItemsClient;
        private readonly ICurrentUserService currentUserService;
        private readonly IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver;

        public Handler(
            YourBrand.Catalog.IItemsClient itemsClient,
            YourBrand.Catalog.IItemGroupsClient itemGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryItemsClient,
            ICurrentUserService currentUserService,
            IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver)
        {
            _itemsClient = itemsClient;
            this.itemGroupsClient = itemGroupsClient;
            _inventoryItemsClient = inventoryItemsClient;
            this.currentUserService = currentUserService;
            this.storeHandleToStoreIdResolver = storeHandleToStoreIdResolver;
        }

        public async Task<ItemsResult<SiteItemDto>> Handle(GetItems request, CancellationToken cancellationToken)
        {
            var storeId = await storeHandleToStoreIdResolver.ToStoreId(currentUserService.Host!);

            var result = await _itemsClient.GetItemsAsync(storeId,
                false, true, request.ItemGroupId, request.ItemGroup2Id,
                request.ItemGroup3Id, request.Page - 1, request.PageSize, request.SearchString,
                request.SortBy, request.SortDirection, cancellationToken);

            List<SiteItemDto> items = new List<SiteItemDto>();
            foreach (var item in result.Items)
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
    }
}
