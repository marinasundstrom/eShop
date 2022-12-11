using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Pricing.Application.Common;
using YourBrand.Pricing.Application.ProductPriceLists.Dtos;

namespace YourBrand.Pricing.Application.ProductPriceLists.Queries;

public record GetProductPriceLists(int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult< ProductPriceListDto>>
{
    public class Handler : IRequestHandler<GetProductPriceLists, ItemsResult<ProductPriceListDto>>
    {
        private readonly IProductPriceListRepository productPriceListRepository;

        public Handler(IProductPriceListRepository productPriceListRepository)
        {
            this.productPriceListRepository = productPriceListRepository;
        }

        public async Task<ItemsResult<ProductPriceListDto>> Handle(GetProductPriceLists request, CancellationToken cancellationToken)
        {
            var query = productPriceListRepository.GetAll();

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var productPriceLists = await query
                .Include(i => i.ProductPrices)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult< ProductPriceListDto>(productPriceLists.Select(x => x.ToDto()), totalCount);
        }
    }
}
