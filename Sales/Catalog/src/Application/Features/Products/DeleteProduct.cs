using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record DeleteProduct(long ProductId) : IRequest
{
    public sealed class Handler : IRequestHandler<DeleteProduct>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            _context.Products.Remove(item);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
