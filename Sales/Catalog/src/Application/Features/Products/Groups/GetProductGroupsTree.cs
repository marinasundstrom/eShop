using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Features.Products.Groups;

namespace YourBrand.Catalog.Controllers;

public record GetProductGroupTree(string? StoreId) : IRequest<IEnumerable<ProductGroupTreeNodeDto>>
{
    public class Handler : IRequestHandler<GetProductGroupTree, IEnumerable<ProductGroupTreeNodeDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductGroupTreeNodeDto>> Handle(GetProductGroupTree request, CancellationToken cancellationToken)
        {
            var itemGroups = await _context.ProductGroups
                .Include(x => x.Parent)
                .ThenInclude(x => x!.Parent)
                .Include(x => x.SubGroups)
                .Where(x => x.Parent == null)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            return itemGroups.Select(x => x.ToDto3());
        }
    }
}