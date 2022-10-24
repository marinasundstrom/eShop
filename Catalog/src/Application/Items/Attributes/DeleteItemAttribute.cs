using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Attributes;

public record DeleteItemAttribute(string ItemId, string AttributeId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemAttribute>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .Include(x => x.Attributes)
                .FirstAsync(x => x.Id == request.ItemId);

            var attribute = item.Attributes
                .First(x => x.Id == request.AttributeId);

            item.Attributes.Remove(attribute);
            _context.Attributes.Remove(attribute);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
