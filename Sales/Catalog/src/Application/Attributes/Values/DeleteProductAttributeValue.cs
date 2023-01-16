using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Attributes.Values;

public record DeleteProductAttributeValue(string Id, string ValueId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductAttributeValue>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductAttributeValue request, CancellationToken cancellationToken)
        {
            var attribute = await _context.Attributes
             .Include(pv => pv.Values)
             .FirstAsync(o => o.Id == request.Id);

            var value = attribute.ProductAttributes.First(o => o.AttributeId == request.ValueId);

            attribute.ProductAttributes.Remove(value);
            _context.ProductAttributes.Remove(value);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
