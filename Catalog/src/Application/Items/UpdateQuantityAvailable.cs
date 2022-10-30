using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application.Items;

public record UpdateQuantityAvailable(string ItemId, int Quantity) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<UpdateQuantityAvailable, ItemDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemDto?> Handle(UpdateQuantityAvailable request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .AsSplitQuery()
                .FirstAsync(p => p.Id == request.ItemId);

            item.QuantityAvailable = request.Quantity;

            await _context.SaveChangesAsync(cancellationToken);

            return item?.ToDto();
        }
    }
}
