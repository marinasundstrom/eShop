using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog;
using YourBrand.Catalog.Common.Models;
using YourBrand.Catalog.Features.Currencies;

namespace YourStore.Catalog.Features.Currencies;

public sealed record GetCurrenciesQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, YourBrand.Catalog.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<CurrencyDto>>
{
    sealed class GetCurrenciesQueryHandler : IRequestHandler<GetCurrenciesQuery, ItemsResult<CurrencyDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetCurrenciesQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<ItemsResult<CurrencyDto>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Currencies
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
                query = query.OrderBy(x => x.Symbol);
            }

            var items = await query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new ItemsResult<CurrencyDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}
