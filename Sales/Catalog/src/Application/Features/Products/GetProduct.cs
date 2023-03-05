using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public record GetProduct(string ProductIdOrHandle) : IRequest<ProductDto?>
{
    public class Handler : IRequestHandler<GetProduct, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            long.TryParse(request.ProductIdOrHandle, out var productId);

            var query = _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .IncludeAll();

            var item = productId == 0 
                ? await query.FirstOrDefaultAsync(p => p.Handle == request.ProductIdOrHandle, cancellationToken) 
                : await query.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken);

            return item?.ToDto();
        }
    }
}
