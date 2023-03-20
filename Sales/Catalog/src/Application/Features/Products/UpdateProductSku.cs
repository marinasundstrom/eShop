using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductSku(long ProductId, string Sku) : IRequest
{
    public sealed class Handler : IRequestHandler<UpdateProductSku>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductSku request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            item.SKU = request.Sku;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
