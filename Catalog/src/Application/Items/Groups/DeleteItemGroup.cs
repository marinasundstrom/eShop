using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Options;

public record DeleteItemGroup(string ItemGroupId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemGroup>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemGroup request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ItemGroups
                .Include(x => x.Items)
                .FirstAsync(x => x.Id == request.ItemGroupId);

            itemGroup.Items.Clear();

            _context.ItemGroups.Remove(itemGroup);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
