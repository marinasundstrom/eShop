using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record GetProduct(string ProductIdOrHandle) : IRequest<ProductDto?>
{
    public sealed class Handler : IRequestHandler<GetProduct, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            bool isProductId = long.TryParse(request.ProductIdOrHandle, out var productId);

            var query = _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .IncludeAll();

            var item = isProductId
                ? await query.FirstOrDefaultAsync(p => p.Id == productId, cancellationToken) 
                : await query.FirstOrDefaultAsync(p => p.Handle == request.ProductIdOrHandle, cancellationToken);

            return item?.ToDto();
        }
    }
}
