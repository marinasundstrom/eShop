
using YourBrand.Inventory.Domain;

using MediatR;
using YourBrand.Inventory.Application.Features.Items;
using Microsoft.EntityFrameworkCore;

namespace YourBrand.Inventory.Application.Features.Items.Groups.Commands;

public record CreateItemGroup(string Name) : IRequest<ItemGroupDto>
{
    public class Handler : IRequestHandler<CreateItemGroup, ItemGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemGroupDto> Handle(CreateItemGroup request, CancellationToken cancellationToken)
        {
            var item = new Domain.Entities.ItemGroup(request.Name);

            _context.ItemGroups.Add(item);

            await _context.SaveChangesAsync(cancellationToken);

            item = await _context.ItemGroups
               .AsNoTracking()
               .FirstAsync(c => c.Id == item.Id);

            return item.ToDto();
        }
    }
}
