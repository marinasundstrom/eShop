using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application.Products.Variants;

public record GetProductVariants(string ProductId, int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Application.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<ProductDto>>
{
    public class Handler : IRequestHandler<GetProductVariants, ItemsResult<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<ProductDto>> Handle(GetProductVariants request, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .Where(pv => pv.ParentProduct!.Id == request.ProductId)
                .OrderBy(x => x.Id)
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

            var variants = await query
                .Include(pv => pv.ParentProduct)
                    .ThenInclude(pv => pv!.Group)
                .Include(pv => pv.AttributeValues)
                    .ThenInclude(pv => pv.Attribute)
                .Include(pv => pv.AttributeValues)
                    .ThenInclude(pv => pv.Value)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new ItemsResult<ProductDto>(variants.Select(item => item.ToDto()), totalCount);
        }
        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
