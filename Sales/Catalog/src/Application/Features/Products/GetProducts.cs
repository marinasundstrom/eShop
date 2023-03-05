using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Common.Models;

namespace YourBrand.Catalog.Features.Products;

public record GetProducts(string? StoreId = null, bool IncludeUnlisted = false, bool GroupProducts = true, string? GroupIdOrHandle = null, string? Group2IdOrHandle = null, string? Group3IdOrHandle = null, int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<ProductDto>>
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

            if (request.GroupIdOrHandle is not null)
            {
                long.TryParse(request.GroupIdOrHandle, out var groupId);

                query = groupId == 0 
                            ? query.Where(x => x.Group!.Handle == request.GroupIdOrHandle)
                            : query.Where(x => x.Group!.Id == groupId);

                if (request.Group2IdOrHandle is not null)
                {
                    long.TryParse(request.Group2IdOrHandle, out var group2Id);
                    
                    query = group2Id == 0 
                        ? query.Where(x => x.Group2!.Handle == request.Group2IdOrHandle)
                        : query.Where(x => x.Group2!.Id == group2Id);

                    if (request.Group3IdOrHandle is not null)
                    {
                        long.TryParse(request.Group3IdOrHandle, out var group3Id);

                         query = group3Id == 0 
                            ? query.Where(x => x.Group3!.Handle == request.Group3IdOrHandle)
                            : query.Where(x => x.Group3!.Id == group3Id);
                    }
                }
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