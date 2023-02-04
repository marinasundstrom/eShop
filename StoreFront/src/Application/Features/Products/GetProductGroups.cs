using MediatR;
using YourBrand.Catalog;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record GetProductGroups(
    string? ParentGroupId, bool IncludeWithUnlisted)
    : IRequest<IEnumerable<ProductGroupDto>>
{
    sealed class Handler : IRequestHandler<GetProductGroups, IEnumerable<ProductGroupDto>>
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

        public async Task<IEnumerable<ProductGroupDto>> Handle(GetProductGroups request, CancellationToken cancellationToken)
        {
            var storeId = await storeHandleToStoreIdResolver.ToStoreId(currentUserService.Host!);
            return await productGroupsClient.GetProductGroupsAsync(storeId, request.ParentGroupId, request.IncludeWithUnlisted, false, cancellationToken);
        }
    }
}
