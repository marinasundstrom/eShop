using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Attributes.Groups;

public record GetItemAttributeGroups(string ItemId) : IRequest<IEnumerable<AttributeGroupDto>>
{
    public class Handler : IRequestHandler<GetItemAttributeGroups, IEnumerable<AttributeGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeGroupDto>> Handle(GetItemAttributeGroups request, CancellationToken cancellationToken)
        {
            var groups = await _context.AttributeGroups
            .AsTracking()
            .Include(x => x.Item)
            .Where(x => x.Item!.Id == request.ItemId)
            .ToListAsync();

            return groups.Select(group => new AttributeGroupDto(group.Id, group.Name, group.Description));
        }
    }
}
