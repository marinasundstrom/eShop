using MassTransit.Mediator;
using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record GetProduct(string ProductIdOrHandle)
    : IRequest<SiteProductDto?>
{
    sealed class Handler : IRequestHandler<GetProduct, SiteProductDto?>
    {
        private readonly IStoresProvider _storesProvider;
        private readonly YourBrand.Catalog.IProductsClient _productsClient;
        private readonly IProductGroupsClient productGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryProductsClient;
        private readonly IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver;

        public Handler(
            IStoresProvider storesProvider,
            YourBrand.Catalog.IProductsClient productsClient,
            YourBrand.Catalog.IProductGroupsClient productGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryProductsClient,
            IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver)
        {
            _storesProvider = storesProvider;
            _productsClient = productsClient;
            this.productGroupsClient = productGroupsClient;
            _inventoryProductsClient = inventoryProductsClient;
            this.storeHandleToStoreIdResolver = storeHandleToStoreIdResolver;
        }

        public async Task<SiteProductDto?> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            var store = await _storesProvider.GetCurrentStore(cancellationToken);

            var product = await _productsClient.GetProductAsync(request.ProductIdOrHandle, cancellationToken);
            /*
            int? available = null;
            try 
            {
                var inventoryProduct = await _inventoryProductsClient.GetProductAsync(product.Id, cancellationToken);
                available = inventoryProduct.QuantityAvailable;
            } catch {}
            */
            return product.ToDto(store!);
        }
    }
}
