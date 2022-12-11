using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Products.Variants;

public record DeleteProductVariant(string ProductId, string ProductVariantId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductVariant>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductVariant request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .AsSplitQuery()
                .Include(pv => pv.Variants)
                .FirstAsync(x => x.Id == request.ProductVariantId);

            var variant = item.Variants.First(x => x.Id == request.ProductVariantId);

            item.Variants.Remove(variant);
            _context.Products.Remove(variant);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
