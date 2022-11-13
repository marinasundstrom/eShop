using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Stores;

public record GetStores(int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Application.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<StoreDto>>
{
    public class Handler : IRequestHandler<GetStores, ItemsResult<StoreDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<StoreDto>> Handle(GetStores request, CancellationToken cancellationToken)
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
                query = query.OrderBy(request.SortBy, request.SortDirection == Application.Common.Models.SortDirection.Desc ? YourBrand.Catalog.Application.SortDirection.Descending : YourBrand.Catalog.Application.SortDirection.Ascending);
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
