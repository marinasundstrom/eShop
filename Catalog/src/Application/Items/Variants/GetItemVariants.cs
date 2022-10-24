using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Common.Models;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Variants;

public record GetItemVariants(string ItemId,  int Page = 10, int PageSize = 10, string? SearchString = null, string? SortBy = null, Application.Common.Models.SortDirection? SortDirection = null) : IRequest<ItemsResult<ItemDto>>
{
    public class Handler : IRequestHandler<GetItemVariants, ItemsResult<ItemDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemsResult<ItemDto>> Handle(GetItemVariants request, CancellationToken cancellationToken)
        {
            var query = _context.Items
                .Where(pv => pv.ParentItem!.Id == request.ItemId)
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
                .Include(pv => pv.ParentItem)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Attribute)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Value)
                .Skip(request.Page * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync();

            return new ItemsResult<ItemDto>(variants.Select(item => {
                return new ItemDto(item.Id, item.Name, item.Description,
                    item.Group is not null ? new Groups.ItemGroupDto(item.Group.Id, item.Group.Name, item.Group.Description, item.Group?.Parent?.Id) : null,
                                GetImageUrl(item.Image), item.Price, item.HasVariants, (ItemVisibility?)item.Visibility,
                                item.AttributeValues.Select(x => x.ToDto()));
            }), totalCount);
        }
    private static string? GetImageUrl(string? name)
    {
        return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
    }
    }
}
