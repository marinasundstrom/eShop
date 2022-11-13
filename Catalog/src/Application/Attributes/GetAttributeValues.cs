using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;

namespace YourBrand.Catalog.Application.Attributes;

public record GetAttributeValues(string AttributeId) : IRequest<IEnumerable<AttributeValueDto>>
{
    public class Handler : IRequestHandler<GetAttributeValues, IEnumerable<AttributeValueDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeValueDto>> Handle(GetAttributeValues request, CancellationToken cancellationToken)
        {
            var options = await _context.AttributeValues
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Attribute)
                .ThenInclude(pv => pv.Group)
                .Where(p => p.Attribute.Id == request.AttributeId)
                .ToArrayAsync();

            return options.Select(x => x.ToDto());
        }
    }
}
