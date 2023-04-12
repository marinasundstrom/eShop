using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateQuantityAvailable(long ProductId, int Quantity) : IRequest<Result<ProductDto>>
{
    public sealed class Handler : IRequestHandler<UpdateQuantityAvailable, Result<ProductDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ProductDto>> Handle(UpdateQuantityAvailable request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure<ProductDto>(Errors.Products.ProductNotFound);
            }

            item.QuantityAvailable = request.Quantity;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(item.ToDto());
        }
    }
}