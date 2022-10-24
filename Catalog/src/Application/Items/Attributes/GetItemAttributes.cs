using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Attributes;

public record GetItemAttributes(string ItemId) : IRequest<IEnumerable<AttributeDto>>
{
    public class Handler : IRequestHandler<GetItemAttributes, IEnumerable<AttributeDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeDto>> Handle(GetItemAttributes request, CancellationToken cancellationToken)
        {
            var attributes = await _context.Attributes
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Group)
                .Include(pv => pv.Values)
                .Where(p => p.Items.Any(x => x.Id == request.ItemId))
                .ToArrayAsync();


            return attributes.Select(x => x.ToDto());
        }
    }
}
