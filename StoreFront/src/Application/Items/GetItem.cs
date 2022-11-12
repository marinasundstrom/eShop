using MassTransit.Mediator;
using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;

namespace YourBrand.StoreFront.Application.Items;

public sealed record GetItem(string Id)
    : IRequest<SiteItemDto?>
{
    sealed class Handler : IRequestHandler<GetItem, SiteItemDto?>
    {
        private readonly YourBrand.Catalog.IItemsClient _itemsClient;
        private readonly IItemGroupsClient itemGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryItemsClient;
        private readonly IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver;

        public Handler(
            YourBrand.Catalog.IItemsClient itemsClient,
            YourBrand.Catalog.IItemGroupsClient itemGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryItemsClient,
            IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver)
        {

            _itemsClient = itemsClient;
            this.itemGroupsClient = itemGroupsClient;
            _inventoryItemsClient = inventoryItemsClient;
            this.storeHandleToStoreIdResolver = storeHandleToStoreIdResolver;
        }

        public async Task<SiteItemDto?> Handle(GetItem request, CancellationToken cancellationToken)
        {
            var item = await _itemsClient.GetItemAsync(request.Id, cancellationToken);
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
    }
}
