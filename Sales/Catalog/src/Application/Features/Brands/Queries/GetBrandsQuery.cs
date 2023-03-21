using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Common.Models;

namespace YourBrand.Catalog.Features.Brands.Queries;

public sealed record GetBrandsQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, Catalog.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<BrandDto>>
{
    sealed class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, ItemsResult<BrandDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetBrandsQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<ItemsResult<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Brand> result = _context
                    .Brands
                     //.OrderBy(o => o.Created)
                     .AsNoTracking()
                     .AsQueryable();

            if (request.SearchString is not null)
            {
                result = result.Where(ca => ca.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await result.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                result = result.OrderBy(request.SortBy, request.SortDirection == Catalog.Common.Models.SortDirection.Desc ? Catalog.SortDirection.Descending : Catalog.SortDirection.Ascending);
            }
            else 
            {
                result = result.OrderBy(x => x.Name);
            }

            var items = await result
                .Skip((request.Page) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<BrandDto>(items.Select(cp => cp.ToDto()), totalCount);
        }
    }
}
