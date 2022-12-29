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

            var value = attribute.Values.First(o => o.Id == request.ValueId);

            attribute.Values.Remove(value);
            _context.AttributeValues.Remove(value);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
