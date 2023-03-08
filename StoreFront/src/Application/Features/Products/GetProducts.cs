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
    int Page = 1,
    int PageSize = 10,
    string? SearchString = null,
    string? SortBy = null,
    YourBrand.Catalog.SortDirection? SortDirection = null)
    : IRequest<ItemsResult<SiteProductDto>>
{
    sealed class Handler : IRequestHandler<GetProducts, ItemsResult<SiteProductDto>>
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

        public async Task<ItemsResult<SiteProductDto>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var storeId = await storeHandleToStoreIdResolver.ToStoreId(currentUserService.Host!);

            var result = await _productsClient.GetProductsAsync(storeId,
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

                products.Add(product.ToDto());
            }
            return new ItemsResult<SiteProductDto>(products, result.TotalItems);
        }
    }
}
