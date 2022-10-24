using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Attributes;

public record DeleteItemAttributeValue(string ItemId, string AttributeId, string ValueId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemAttributeValue>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemAttributeValue request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
             .AsSplitQuery()
             .Include(pv => pv.Attributes)
             .ThenInclude(pv => pv.Values)
             .FirstAsync(p => p.Id == request.ItemId);

            var attribute = item.Attributes.First(o => o.Id == request.AttributeId);

            var value = attribute.Values.First(o => o.Id == request.ValueId);

            attribute.Values.Remove(value);
            _context.AttributeValues.Remove(value);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
