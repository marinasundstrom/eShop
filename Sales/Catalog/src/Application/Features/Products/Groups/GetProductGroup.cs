using System.Net;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record GetProductGroup(string ProductGroupIdOrPath) : IRequest<ProductGroupDto?>
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
            string decoded = WebUtility.UrlDecode(request.ProductGroupIdOrPath);

            bool isProductId = long.TryParse(decoded, out var productGroupId);

            var query = _context.ProductGroups
                .Include(x => x.Parent)
                .AsNoTracking();

            var itemGroup = isProductId 
                ? await query.FirstOrDefaultAsync(p => p.Id == productGroupId, cancellationToken) 
                : await query.FirstOrDefaultAsync(p => p.Path == decoded, cancellationToken);

            return itemGroup?.ToDto();
        }
    }
}