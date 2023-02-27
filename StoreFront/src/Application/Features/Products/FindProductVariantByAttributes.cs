using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Features.Carts;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record FindProductVariantByAttributes(
    string Id, Dictionary<string, string> Attributes)
    : IRequest<SiteProductDto?>
{
    sealed class Handler : IRequestHandler<FindProductVariantByAttributes, SiteProductDto?>
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

        public async Task<SiteProductDto?> Handle(FindProductVariantByAttributes request, CancellationToken cancellationToken)
        {
            var r = await _productsClient.FindVariantByAttributeValuesAsync(request.Id, request.Attributes!, cancellationToken);
            return r?.ToDto();
        }
    }
}
