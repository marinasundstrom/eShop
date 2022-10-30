using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Groups;

public record GetItemGroups(string? ParentGroupId, bool IncludeWithUnlistedItems, bool IncludeJustTopLevel) : IRequest<IEnumerable<ItemGroupDto>>
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

            if(request.ParentGroupId is not null) 
            {
                query = query.Where(x => x.Parent!.Id == request.ParentGroupId);        
            }
            else 
            {
                if(request.IncludeJustTopLevel) 
                {
                    query = query.Where(x => x.Parent == null);
                }
            }

            if (!request.IncludeWithUnlistedItems)
            {
                query = query.Where(x => x.Items.Any(z => z.Visibility == Domain.Enums.ItemVisibility.Listed));
            }

            var itemGroups = await query.ToListAsync();

            return itemGroups.Select(group => new ItemGroupDto(group.Id, group.Name, group.Description, group?.Parent?.Id));
        }
    }
}
