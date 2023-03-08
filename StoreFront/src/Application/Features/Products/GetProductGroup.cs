using MediatR;

using YourBrand.Catalog;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record GetProductGroup(
    string ProductGroupIdOrPath)
    : IRequest<ProductGroupDto>
{
    sealed class Handler : IRequestHandler<GetProductGroup, ProductGroupDto>
    {
        private readonly IProductGroupsClient productGroupsClient;
        private readonly ICurrentUserService currentUserService;
        private readonly IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver;

        public Handler(
            YourBrand.Catalog.IProductGroupsClient productGroupsClient,
            ICurrentUserService currentUserService,
            IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver)
        {

            this.productGroupsClient = productGroupsClient;
            this.currentUserService = currentUserService;
            this.storeHandleToStoreIdResolver = storeHandleToStoreIdResolver;
        }

        public async Task<ProductGroupDto> Handle(GetProductGroup request, CancellationToken cancellationToken)
        {
            var storeId = await storeHandleToStoreIdResolver.ToStoreId(currentUserService.Host!);
            return await productGroupsClient.GetProductGroupAsync(request.ProductGroupIdOrPath, cancellationToken);
        }
    }
}
