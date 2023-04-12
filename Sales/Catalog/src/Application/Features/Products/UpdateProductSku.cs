using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductSku(long ProductId, string Sku) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductSku, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductSku request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            if (item.SKU == request.Sku) 
            {
                return Result.Failure(Errors.Products.ProductAlreadyHasSpecifiedSku);
            }

            var existing = await _context.Products
                .AnyAsync(x => x.SKU == request.Sku);

            if (existing) 
            {
                return Result.Failure(Errors.Products.ProductWithSkuAlreadyExists);
            }

            item.SKU = request.Sku;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
