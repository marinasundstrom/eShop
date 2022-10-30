using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Groups;

public record GetItemGroup(string ItemGroupId) : IRequest<ItemGroupDto?>
{
    public class Handler : IRequestHandler<GetItemGroup, ItemGroupDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemGroupDto?> Handle(GetItemGroup request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ItemGroups
                .Include(x => x.Parent)
                .Include(x => x.Items)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ItemGroupId);

            return itemGroup?.ToDto();
        }
    }
}
