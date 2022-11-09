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

        public Handler(
            YourBrand.Catalog.IItemsClient itemsClient,
            YourBrand.Catalog.IItemGroupsClient itemGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryItemsClient,
            ICurrentUserService currentUserService)
        {

            _itemsClient = itemsClient;
            this.itemGroupsClient = itemGroupsClient;
            _inventoryItemsClient = inventoryItemsClient;
            this.currentUserService = currentUserService;
        }

        public async Task<IEnumerable<ItemGroupDto>> Handle(GetItemGroups request, CancellationToken cancellationToken)
        {
            return await itemGroupsClient.GetItemGroupsAsync(currentUserService.Host, request.ParentGroupId, request.IncludeWithUnlisted, false, cancellationToken);
        }
    }
}
