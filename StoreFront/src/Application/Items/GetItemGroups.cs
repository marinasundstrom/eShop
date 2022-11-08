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

        public Handler(
            YourBrand.Catalog.IItemsClient itemsClient,
            YourBrand.Catalog.IItemGroupsClient itemGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryItemsClient)
        {

            _itemsClient = itemsClient;
            this.itemGroupsClient = itemGroupsClient;
            _inventoryItemsClient = inventoryItemsClient;
        }

        public async Task<IEnumerable<ItemGroupDto>> Handle(GetItemGroups request, CancellationToken cancellationToken)
        {
            return await itemGroupsClient.GetItemGroupsAsync(request.ParentGroupId, request.IncludeWithUnlisted, false, cancellationToken);
        }
    }
}
