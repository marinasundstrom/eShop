using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Attributes;

public record DeleteProductAttribute(string ProductId, string AttributeId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductAttribute>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .Include(x => x.ProductAttributes)
                .FirstAsync(x => x.Id == request.ProductId);

            var attribute = item.ProductAttributes
                .First(x => x.AttributeId == request.AttributeId);

            item.ProductAttributes.Remove(attribute);
            _context.ProductAttributes.Remove(attribute);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}