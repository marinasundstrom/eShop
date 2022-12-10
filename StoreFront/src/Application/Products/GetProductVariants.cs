using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Carts;
using YourBrand.StoreFront.Application.Common.Models;

namespace YourBrand.StoreFront.Application.Products;

public sealed record GetProductVariants(
    string Id, int Page = 1, int PageSize = 10,
    string? SearchString = null, string? SortBy = null, YourBrand.Catalog.SortDirection? SortDirection = null)
    : IRequest<ItemsResult<SiteProductDto>>
{
    sealed class Handler : IRequestHandler<GetProductVariants, ItemsResult<SiteProductDto>>
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

        public async Task<ItemsResult<SiteProductDto>> Handle(GetProductVariants request, CancellationToken cancellationToken)
        {
            var result = await _productsClient.GetVariantsAsync(request.Id, request.Page - 1, request.PageSize, request.SearchString, request.SortBy, request.SortDirection, cancellationToken);

            return new ItemsResult<SiteProductDto>(result.Items.Select(x => x.ToDto()), result.TotalItems);
        }
    }
}
