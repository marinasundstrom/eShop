using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Variants;

public record DeleteItemVariant(string ItemId, string ItemVariantId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemVariant>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemVariant request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .AsSplitQuery()
                .Include(pv => pv.Variants)
                .FirstAsync(x => x.Id == request.ItemVariantId);

            var variant = item.Variants.First(x => x.Id == request.ItemVariantId);

            item.Variants.Remove(variant);
            _context.Items.Remove(variant);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
