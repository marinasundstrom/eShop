using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Products.Groups;
using YourBrand.Catalog.Application.Products.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application.Products;

public record UpdateQuantityAvailable(string ProductId, int Quantity) : IRequest<ProductDto?>
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
