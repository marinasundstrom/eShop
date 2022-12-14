using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Application.Products.Groups;
using YourBrand.Catalog.Application.Products.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application.Products;

public record GetProducts(string? StoreId = null, bool IncludeUnlisted = false, bool GroupProducts = true, string? GroupId = null, string? Group2Id = null, string? Group3Id = null, int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Application.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<ProductDto>>
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
                .Include(pv => pv.ParentProduct)
                .ThenInclude(pv => pv!.Group)
                .Include(pv => pv.Group)
                .Include(pv => pv.Attributes)
                .ThenInclude(pv => pv.Values)
                .Include(pv => pv.Options)
                .ThenInclude(pv => pv.Values)
                .Include(pv => pv.Options)
                .ThenInclude(pv => pv.DefaultValue)
                .AsQueryable();

            if (request.StoreId is not null)
            {
                query = query.Where(x => x.StoreId == request.StoreId);
            }

            if (!request.IncludeUnlisted)
            {
                query = query.Where(x => x.Visibility == Domain.Enums.ProductVisibility.Listed);
            }

            if (request.GroupId is not null)
            {
                query = query.Where(x => x.Group!.Id == request.GroupId);

                if (request.Group2Id is not null)
                {
                    query = query.Where(x => x.Group2!.Id == request.Group2Id);

                    if (request.Group2Id is not null)
                    {
                        query = query.Where(x => x.Group3!.Id == request.Group3Id);
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

            return new ItemsResult<ProductDto>(items.Select(item => item.ToDto()),
            totalCount);
        }
    }
}
