using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog;
using YourBrand.Catalog.Common.Models;
using YourBrand.Catalog.Features.Stores;

namespace YourStore.Catalog.Features.Stores.Queries;

public sealed record GetStoresQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, YourBrand.Catalog.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<StoreDto>>
{
    sealed class GetStoresQueryHandler : IRequestHandler<GetStoresQuery, ItemsResult<StoreDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetStoresQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<ItemsResult<StoreDto>> Handle(GetStoresQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Stores
                .AsSplitQuery()
                .AsNoTracking()
                .AsQueryable();

            if (request.SearchString is not null)
            {
                query = query.Where(ca => ca.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await query.CountAsync();

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection == YourBrand.Catalog.Common.Models.SortDirection.Desc ? YourBrand.Catalog.SortDirection.Descending : YourBrand.Catalog.SortDirection.Ascending);
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }

            var items = await query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new ItemsResult<StoreDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}
