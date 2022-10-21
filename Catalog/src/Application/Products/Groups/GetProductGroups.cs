using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Domain;

namespace Catalog.Application.Products.Groups;

public record GetProductGroups(bool IncludeWithUnlistedProducts) : IRequest<IEnumerable<ProductGroupDto>>
{
    public class Handler : IRequestHandler<GetProductGroups, IEnumerable<ProductGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductGroupDto>> Handle(GetProductGroups request, CancellationToken cancellationToken)
        {
            var query = _context.ProductGroups
                    .Include(x => x.Products)
                    .AsQueryable();

            if (!request.IncludeWithUnlistedProducts)
            {
                query = query.Where(x => x.Products.Any(z => z.Visibility == Domain.Enums.ProductVisibility.Listed));
            }

            var productGroups = await query.ToListAsync();

            return productGroups.Select(group => new ProductGroupDto(group.Id, group.Name, group.Description, group?.Parent?.Id));
        }
    }
}
