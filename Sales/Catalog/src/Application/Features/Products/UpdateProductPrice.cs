using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductPrice(long ProductId, decimal Price) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductPrice, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductPrice request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            item.Price = request.Price;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}