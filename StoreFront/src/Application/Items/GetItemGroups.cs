using MediatR;
using YourBrand.Catalog;

namespace YourBrand.StoreFront.Application.Items;

public sealed record GetItemGroups(
    string? ParentGroupId, bool IncludeWithUnlisted)
    : IRequest<IEnumerable<ItemGroupDto>>
{
    sealed class Handler : IRequestHandler<GetItemGroups, IEnumerable<ItemGroupDto>>
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

        public async Task<IEnumerable<ItemGroupDto>> Handle(GetItemGroups request, CancellationToken cancellationToken)
        {
            var storeId = await storeHandleToStoreIdResolver.ToStoreId(currentUserService.Host!);
            return await itemGroupsClient.GetItemGroupsAsync(storeId, request.ParentGroupId, request.IncludeWithUnlisted, false, cancellationToken);
        }
    }
}
