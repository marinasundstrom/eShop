using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Application.Attributes;
using Catalog.Domain;

namespace Catalog.Application.Products.Attributes;

public record GetProductAttributes(string ProductId) : IRequest<IEnumerable<AttributeDto>>
{
    public class Handler : IRequestHandler<GetProductAttributes, IEnumerable<AttributeDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeDto>> Handle(GetProductAttributes request, CancellationToken cancellationToken)
        {
            var attributes = await _context.Attributes
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Group)
                .Include(pv => pv.Values)
                .Where(p => p.Products.Any(x => x.Id == request.ProductId))
                .ToArrayAsync();


            return attributes.Select(x => x.ToDto());
        }
    }
}
