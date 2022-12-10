using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Products.Groups;

public record GetProductGroup(string ProductGroupId) : IRequest<ProductGroupDto?>
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
            var itemGroup = await _context.ProductGroups
                .Include(x => x.Parent)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ProductGroupId);

            return itemGroup?.ToDto();
        }
    }
}
