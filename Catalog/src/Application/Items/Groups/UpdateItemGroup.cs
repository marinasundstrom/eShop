using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Groups;

public record UpdateItemGroup(string ItemGroupId, ApiUpdateItemGroup Data) : IRequest<ItemGroupDto>
{
    public class Handler : IRequestHandler<UpdateItemGroup, ItemGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemGroupDto> Handle(UpdateItemGroup request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ItemGroups
                    .FirstAsync(x => x.Id == request.ItemGroupId);

            var parentGroup = await _context.ItemGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.ParentGroupId);

            itemGroup.Name = request.Data.Name;
            itemGroup.Description = request.Data.Description;
            itemGroup.Parent = parentGroup;

            await _context.SaveChangesAsync();

            return new ItemGroupDto(itemGroup.Id, itemGroup.Name, itemGroup.Description, itemGroup?.Parent?.Id);
        }
    }
}
