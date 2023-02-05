
using YourBrand.Inventory.Domain;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace YourBrand.Inventory.Application.Features.Warehouses.Items.Commands;

public record ShipWarehouseItems(string Id, int Quantity, bool FromPicked = false) : IRequest
{
    public class Handler : IRequestHandler<ShipWarehouseItems>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ShipWarehouseItems request, CancellationToken cancellationToken)
        {
            var item = await _context.WarehouseItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item is null) throw new Exception();

            item.Ship(request.Quantity, request.FromPicked);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
