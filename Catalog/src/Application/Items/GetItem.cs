using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application.Items;

public record GetItem(string ItemId) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<GetItem, ItemDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemDto?> Handle(GetItem request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Group)
                .Include(pv => pv.Attributes)
                .ThenInclude(pv => pv.Values)
                .Include(pv => pv.Options)
                .ThenInclude(pv => pv.Values)
                .Include(pv => pv.Options)
                .ThenInclude(pv => pv.DefaultValue)
                .FirstOrDefaultAsync(p => p.Id == request.ItemId);

            return item?.ToDto();
        }
    }
}
