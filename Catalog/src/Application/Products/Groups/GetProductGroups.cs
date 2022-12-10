using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Products.Groups;

public record GetProductGroups(string? StoreId, string? ParentGroupId, bool IncludeWithUnlistedProducts, bool IncludeHidden) : IRequest<IEnumerable<ProductGroupDto>>
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
                    .AsQueryable();

            query = query.Where(x => x.Parent!.Id == request.ParentGroupId);

            if (request.StoreId is not null)
            {
                query = query.Where(x => x.StoreId == request.StoreId);
            }

            if (!request.IncludeHidden)
            {
                query = query.Where(x => !x.Hidden);
            }

            if (!request.IncludeWithUnlistedProducts)
            {
                query = query.Where(x => x.Products.Any(z => z.Visibility == Domain.Enums.ProductVisibility.Listed));
            }

            var itemGroups = await query
                .Include(x => x.Parent)
                .ToListAsync();

            return itemGroups.Select(group => group.ToDto());
        }
    }
}
