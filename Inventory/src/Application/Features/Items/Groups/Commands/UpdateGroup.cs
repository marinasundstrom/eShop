
using YourBrand.Inventory.Domain;

using MediatR;
using YourBrand.Inventory.Application.Features.Warehouses.Items;
using Microsoft.EntityFrameworkCore;

namespace YourBrand.Inventory.Application.Features.Items.Groups.Commands;

public record UpdateItemGroup(string Id, string Name) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemGroup>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateItemGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.ItemGroups.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item is null) throw new Exception();

            item.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
