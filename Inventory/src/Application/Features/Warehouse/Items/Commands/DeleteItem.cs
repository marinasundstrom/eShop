using YourBrand.Inventory.Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Inventory.Application.Features.Warehouses.Items.Commands;

public record DeleteWarehouseItem(string WarehouseItemId) : IRequest
{
    public class Handler : IRequestHandler<DeleteWarehouseItem>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWarehouseItem request, CancellationToken cancellationToken)
        {
            /*
            var invoice = await _context.WarehouseItems
                //.Include(i => i.Addresses)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.WarehouseItemId, cancellationToken);

            if(invoice is null)
            {
                throw new Exception();
            }

            _context.WarehouseItems.Remove(invoice);

            await _context.SaveChangesAsync(cancellationToken);
            */

            return Unit.Value;
        }
    }
}