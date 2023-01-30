using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Attributes.Groups;

public record GetAttributeGroups() : IRequest<IEnumerable<AttributeGroupDto>>
{
    public class Handler : IRequestHandler<GetAttributeGroups, IEnumerable<AttributeGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeGroupDto>> Handle(GetAttributeGroups request, CancellationToken cancellationToken)
        {
            var groups = await _context.AttributeGroups
            .AsTracking()
            .Include(x => x.Product)
            .ToListAsync();

            return groups.Select(group => new AttributeGroupDto(group.Id, group.Name, group.Description));
        }
    }
}