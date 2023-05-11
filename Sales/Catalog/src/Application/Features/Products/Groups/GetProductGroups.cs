using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Common.Models;

namespace YourBrand.Catalog.Features.Products.Groups;

public record GetProductGroups(string? StoreId, long? ParentGroupId, bool IncludeWithUnlistedProducts, bool IncludeHidden, 
    int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<ProductGroupDto>>
{
    public class Handler : IRequestHandler<GetProductGroups, ItemsResult<ProductGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<ProductGroupDto>> Handle(GetProductGroups request, CancellationToken cancellationToken)
        {
            var query = _context.ProductGroups
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable();

            query = query.Where(x => x.Parent!.Id == request.ParentGroupId);

            if (request.StoreId is not null)
            {
                query = query.Where(x => x.StoreId == request.StoreId);
            }

            if (!request.IncludeHidden)
            {
                query = query.Where(x => !x.Hidden);
            }

            var doNotIncludeWithUnlisted = !request.IncludeWithUnlistedProducts;
            if (doNotIncludeWithUnlisted)
            {
                query = query.Where(x => x.Products.Any() 
                && !x.Products.All(z => z.Visibility == Domain.Enums.ProductVisibility.Unlisted));
            }

            if (request.SearchString is not null)
            {
                query = query.Where(ca => ca.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await query.CountAsync();

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection == Common.Models.SortDirection.Desc ? YourBrand.Catalog.SortDirection.Descending : YourBrand.Catalog.SortDirection.Ascending);
            }
            else
            {
                query = query.OrderBy(x => x.Name);
            }

            var items = await query
                .Include(x => x.Parent)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new ItemsResult<ProductGroupDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}