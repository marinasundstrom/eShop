using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record GetProductGroup(string ProductGroupIdOrHandle) : IRequest<ProductGroupDto?>
{
    public class Handler : IRequestHandler<GetProductGroup, ProductGroupDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductGroupDto?> Handle(GetProductGroup request, CancellationToken cancellationToken)
        {
            long.TryParse(request.ProductGroupIdOrHandle, out var productGroupId);

            var query = _context.ProductGroups
                .Include(x => x.Parent)
                .AsNoTracking();

            var itemGroup = productGroupId == 0 
                ? await query.FirstOrDefaultAsync(p => p.Handle == request.ProductGroupIdOrHandle, cancellationToken) 
                : await query.FirstOrDefaultAsync(p => p.Id == productGroupId, cancellationToken);

            return itemGroup?.ToDto();
        }
    }
}