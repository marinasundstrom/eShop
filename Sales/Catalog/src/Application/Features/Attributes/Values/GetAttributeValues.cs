using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Attributes.Values;

public record GetAttributeValues(string Id) : IRequest<IEnumerable<AttributeValueDto>>
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
                .Where(p => p.Attribute.Id == request.Id)
                .ToArrayAsync();

            return options.Select(x => x.ToDto());
        }
    }
}