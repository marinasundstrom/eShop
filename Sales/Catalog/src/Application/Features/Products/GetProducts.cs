using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Common.Models;

namespace YourBrand.Catalog.Features.Products;

public record GetProducts(string? StoreId = null, bool IncludeUnlisted = false, bool GroupProducts = true, string? ProductGroupIdOrPath = null, int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<ProductDto>>
{
    public class Handler : IRequestHandler<GetProducts, ItemsResult<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<ProductDto>> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .IncludeAll();

            if (request.StoreId is not null)
            {
                query = query.Where(x => x.StoreId == request.StoreId);
            }

            if (!request.IncludeUnlisted)
            {
                query = query.Where(x => x.Visibility == Domain.Enums.ProductVisibility.Listed);
            }

            if (request.ProductGroupIdOrPath is not null)
            {
                bool isProductGroupId = long.TryParse(request.ProductGroupIdOrPath, out var groupId);

                query = isProductGroupId 
                            ? query.Where(x => x.Group!.Id == groupId)
                            : query.Where(x => 
                                x.Group!.Path.StartsWith(request.ProductGroupIdOrPath));
            }

            if (request.GroupProducts)
            {
                query = query.Where(x => x.ParentProductId == null);
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
                query = query.OrderBy(x => x.Id);
            }

            var items = await query
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new ItemsResult<ProductDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}