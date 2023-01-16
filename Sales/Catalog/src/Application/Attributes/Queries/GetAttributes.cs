using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Application.Products;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Attributes;

public record GetAttributes(string[]? Ids = null, int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Application.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<AttributeDto>>
{
    public class Handler : IRequestHandler<GetAttributes, ItemsResult<AttributeDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<AttributeDto>> Handle(GetAttributes request, CancellationToken cancellationToken)
        {
            var query = _context.Attributes
                .AsSplitQuery()
                .AsNoTracking()
                .Include(o => o.Group)
                .Include(o => o.Values)
                .AsQueryable();

            if (request.Ids?.Any() ?? false)
            {
                var ids = request.Ids;
                query = query.Where(o => ids.Any(x => x == o.Id));
            }

            if (request.SearchString is not null)
            {
                query = query.Where(o => o.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                            .OrderBy(x => x.Name)
                            .Include(x => x.Values)
                            .Skip(request.Page * request.PageSize)
                            .Take(request.PageSize).AsQueryable()
                            .ToArrayAsync();

            return new ItemsResult<AttributeDto>(items.Select(item => item.ToDto()), totalCount);
        }
    }
}
