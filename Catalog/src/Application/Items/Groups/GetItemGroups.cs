using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Groups;

public record GetItemGroups(bool IncludeWithUnlistedItems) : IRequest<IEnumerable<ItemGroupDto>>
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
                    .Include(x => x.Items)
                    .AsQueryable();

            if (!request.IncludeWithUnlistedItems)
            {
                query = query.Where(x => x.Items.Any(z => z.Visibility == Domain.Enums.ItemVisibility.Listed));
            }

            var itemGroups = await query.ToListAsync();

            return itemGroups.Select(group => new ItemGroupDto(group.Id, group.Name, group.Description, group?.Parent?.Id));
        }
    }
}
