using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;

namespace YourBrand.StoreFront.Application.Products;

public sealed record FindProductVariantByAttributes2(
    string Id, Dictionary<string, string> Attributes)
    : IRequest<IEnumerable<SiteProductDto>>
{
    sealed class Handler : IRequestHandler<FindProductVariantByAttributes2, IEnumerable<SiteProductDto>>
    {
        private readonly YourBrand.Catalog.IProductsClient _productsClient;
        private readonly IProductGroupsClient productGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryProductsClient;

        public Handler(
            YourBrand.Catalog.IProductsClient productsClient,
            YourBrand.Catalog.IProductGroupsClient productGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryProductsClient)
        {

            _productsClient = productsClient;
            this.productGroupsClient = productGroupsClient;
            _inventoryProductsClient = inventoryProductsClient;
        }

        public async Task<IEnumerable<SiteProductDto>> Handle(FindProductVariantByAttributes2 request, CancellationToken cancellationToken)
        {
            var r = await _productsClient.FindVariantByAttributeValues2Async(request.Id, request.Attributes, cancellationToken);
            return r.Select(x => x.ToDto());
        }
    }
}
