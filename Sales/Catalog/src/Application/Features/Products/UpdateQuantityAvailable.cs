using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public record UpdateQuantityAvailable(long ProductId, int Quantity) : IRequest<ProductDto?>
{
    public class Handler : IRequestHandler<UpdateQuantityAvailable, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(UpdateQuantityAvailable request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .AsSplitQuery()
                .FirstAsync(p => p.Id == request.ProductId);

            item.QuantityAvailable = request.Quantity;

            await _context.SaveChangesAsync(cancellationToken);

            return item?.ToDto();
        }
    }
}