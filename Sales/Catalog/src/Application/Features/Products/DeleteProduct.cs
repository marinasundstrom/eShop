using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record DeleteProduct(long ProductId) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<DeleteProduct, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .Include(x => x.Group)
                .ThenInclude(x => x!.Parent)
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            item.Group?.DecrementProductCount();

            _context.Products.Remove(item);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
