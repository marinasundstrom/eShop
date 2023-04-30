using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Features.Carts;
using YourBrand.StoreFront.Application.Common.Models;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record GetProductVariants(
    string ProductIdOrHandle, int Page = 1, int PageSize = 10,
    string? SearchString = null, string? SortBy = null, YourBrand.Catalog.SortDirection? SortDirection = null)
    : IRequest<ItemsResult<SiteProductDto>>
{
    sealed class Handler : IRequestHandler<GetProductVariants, ItemsResult<SiteProductDto>>
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

        public async Task<ItemsResult<SiteProductDto>> Handle(GetProductVariants request, CancellationToken cancellationToken)
        {
            var store = await _storesProvider.GetCurrentStore(cancellationToken);

            var result = await _productsClient.GetVariantsAsync(request.ProductIdOrHandle, request.Page - 1, request.PageSize, request.SearchString, request.SortBy, request.SortDirection, cancellationToken);

            return new ItemsResult<SiteProductDto>(result.Items.Select(x => x.ToDto(store!)), result.TotalItems);
        }
    }
}
