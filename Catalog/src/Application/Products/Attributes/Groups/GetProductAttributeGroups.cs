using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Application.Attributes;
using Catalog.Domain;

namespace Catalog.Application.Products.Attributes.Groups;

public record GetProductAttributeGroups(string ProductId) : IRequest<IEnumerable<AttributeGroupDto>>
{
    public class Handler : IRequestHandler<GetProductAttributeGroups, IEnumerable<AttributeGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeGroupDto>> Handle(GetProductAttributeGroups request, CancellationToken cancellationToken)
        {
            var groups = await _context.AttributeGroups
            .AsTracking()
            .Include(x => x.Product)
            .Where(x => x.Product!.Id == request.ProductId)
            .ToListAsync();

            return groups.Select(group => new AttributeGroupDto(group.Id, group.Name, group.Description));
        }
    }
}
