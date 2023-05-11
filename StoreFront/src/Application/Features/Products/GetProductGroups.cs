using MediatR;

using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Common.Models;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record GetProductGroups(
    long? ParentGroupId, bool IncludeWithUnlisted)
    : IRequest<ItemsResult<ProductGroupDto>>
{
    sealed class Handler : IRequestHandler<GetProductGroups, ItemsResult<ProductGroupDto>>
    {
        private readonly YourBrand.Catalog.IProductsClient _productsClient;
        private readonly IProductGroupsClient productGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryProductsClient;
        private readonly ICurrentUserService currentUserService;
        private readonly IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver;

        public Handler(
            YourBrand.Catalog.IProductsClient productsClient,
            YourBrand.Catalog.IProductGroupsClient productGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryProductsClient,
            ICurrentUserService currentUserService,
            IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver)
        {

            _productsClient = productsClient;
            this.productGroupsClient = productGroupsClient;
            _inventoryProductsClient = inventoryProductsClient;
            this.currentUserService = currentUserService;
            this.storeHandleToStoreIdResolver = storeHandleToStoreIdResolver;
        }

        public async Task<ItemsResult<ProductGroupDto>> Handle(GetProductGroups request, CancellationToken cancellationToken)
        {
            var storeId = await storeHandleToStoreIdResolver.ToStoreId(currentUserService.Host!);
            var results = await productGroupsClient.GetProductGroupsAsync(storeId, request.ParentGroupId, request.IncludeWithUnlisted, false, null, null, null, null, null, cancellationToken);
            return new ItemsResult<ProductGroupDto>(results.Items, results.TotalItems);
        }
    }
}
