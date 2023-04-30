using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record FindProductVariantByAttributes2(
    string ProductIdOrHandle, Dictionary<string, string> Attributes)
    : IRequest<IEnumerable<SiteProductDto>>
{
    sealed class Handler : IRequestHandler<FindProductVariantByAttributes2, IEnumerable<SiteProductDto>>
    {
        private readonly IStoresProvider _storesProvider;
        private readonly YourBrand.Catalog.IProductsClient _productsClient;
        private readonly IProductGroupsClient productGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryProductsClient;

        public Handler(
            IStoresProvider storesProvider,
            YourBrand.Catalog.IProductsClient productsClient,
            YourBrand.Catalog.IProductGroupsClient productGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryProductsClient)
        {
            _storesProvider = storesProvider;
            _productsClient = productsClient;
            this.productGroupsClient = productGroupsClient;
            _inventoryProductsClient = inventoryProductsClient;
        }

        public async Task<IEnumerable<SiteProductDto>> Handle(FindProductVariantByAttributes2 request, CancellationToken cancellationToken)
        {
            var store = await _storesProvider.GetCurrentStore(cancellationToken);

            var r = await _productsClient.FindVariantByAttributeValues2Async(request.ProductIdOrHandle, request.Attributes!, cancellationToken);
            return r.Select(x => x.ToDto(store!));
        }
    }
}
