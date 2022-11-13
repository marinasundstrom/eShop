using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Groups;

public record GetItemGroups(string? StoreId, string? ParentGroupId, bool IncludeWithUnlistedItems, bool IncludeHidden) : IRequest<IEnumerable<ItemGroupDto>>
{
    public class Handler : IRequestHandler<GetItemGroups, IEnumerable<ItemGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemGroupDto>> Handle(GetItemGroups request, CancellationToken cancellationToken)
        {
            var query = _context.ItemGroups
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

            if (!request.IncludeWithUnlistedItems)
            {
                query = query.Where(x => x.Items.Any(z => z.Visibility == Domain.Enums.ItemVisibility.Listed));
            }

            var itemGroups = await query
                .Include(x => x.Parent)
                .ToListAsync();

            return itemGroups.Select(group => group.ToDto());
        }
    }
}
