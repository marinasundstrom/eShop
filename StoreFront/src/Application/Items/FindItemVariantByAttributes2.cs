using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;

namespace YourBrand.StoreFront.Application.Items;

public sealed record FindItemVariantByAttributes2(
    string Id, Dictionary<string, string> Attributes)
    : IRequest<IEnumerable<SiteItemDto>>
{
    sealed class Handler : IRequestHandler<FindItemVariantByAttributes2, IEnumerable<SiteItemDto>>
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

        public async Task<IEnumerable<SiteItemDto>> Handle(FindItemVariantByAttributes2 request, CancellationToken cancellationToken)
        {
            var r = await _itemsClient.FindVariantByAttributeValues2Async(request.Id, request.Attributes, cancellationToken);
            return r.Select(x => x.ToDto());
        }
    }
}
