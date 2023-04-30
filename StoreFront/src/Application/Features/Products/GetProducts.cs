using System;
using System.Globalization;
using MediatR;
using YourBrand.Catalog;
using YourBrand.StoreFront.Application.Features.Carts;
using YourBrand.StoreFront.Application.Common.Models;
using YourBrand.StoreFront.Application.Services;

namespace YourBrand.StoreFront.Application.Features.Products;

public sealed record GetProducts(
    string? ProductGroupIdOrPath = null,
    string? BrandIdOrHandle = null,
    int Page = 1,
    int PageSize = 10,
    string? SearchString = null,
    string? SortBy = null,
    YourBrand.Catalog.SortDirection? SortDirection = null)
    : IRequest<ItemsResult<SiteProductDto>>
{
    sealed class Handler : IRequestHandler<GetProducts, ItemsResult<SiteProductDto>>
    {
        private readonly IStoresProvider _storesProvider;
        private readonly YourBrand.Catalog.IProductsClient _productsClient;
        private readonly IProductGroupsClient productGroupsClient;
        private readonly YourBrand.Inventory.IItemsClient _inventoryProductsClient;
        private readonly ICurrentUserService currentUserService;
        private readonly IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver;

        public Handler(
            IStoresProvider storesProvider,
            YourBrand.Catalog.IProductsClient productsClient,
            YourBrand.Catalog.IProductGroupsClient productGroupsClient,
            YourBrand.Inventory.IItemsClient inventoryProductsClient,
            ICurrentUserService currentUserService,
            IStoreHandleToStoreIdResolver storeHandleToStoreIdResolver)
        {
            _storesProvider = storesProvider;
            _productsClient = productsClient;
            this.productGroupsClient = productGroupsClient;
            _inventoryProductsClient = inventoryProductsClient;
            this.currentUserService = currentUserService;
            this.storeHandleToStoreIdResolver = storeHandleToStoreIdResolver;
        }

        public async Task<ItemsResult<SiteProductDto>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var store = await _storesProvider.GetCurrentStore(cancellationToken);

            var result = await _productsClient.GetProductsAsync(store!.Id, request.BrandIdOrHandle,
                false, true, request.ProductGroupIdOrPath, 
                request.Page - 1, request.PageSize, request.SearchString,
                request.SortBy, request.SortDirection, cancellationToken);

            List<SiteProductDto> products = new List<SiteProductDto>();
            foreach (var product in result.Items)
            {
                /*
                int? available = null;
                try 
                {
                    var inventoryProduct = await _inventoryProductsClient.GetProductAsync(product.Id, cancellationToken);
                    available = inventoryProduct.QuantityAvailable;
                } catch {}
                */

                products.Add(product.ToDto(store!));
            }
            return new ItemsResult<SiteProductDto>(products, result.TotalItems);
        }
    }
}
