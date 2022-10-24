using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Groups;

public record CreateItemGroup(string ItemId, ApiCreateItemGroup Data) : IRequest<ItemGroupDto>
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
            var parentGroup = await _context.ItemGroups
            .FirstOrDefaultAsync(x => x.Id == request.Data.ParentGroupId);

            var itemGroup = new ItemGroup()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Data.Name,
                Description = request.Data.Description,
                Parent = parentGroup
            };

            _context.ItemGroups.Add(itemGroup);

            await _context.SaveChangesAsync();

            return new ItemGroupDto(itemGroup.Id, itemGroup.Name, itemGroup.Description, itemGroup?.Parent?.Id);

        }
    }
}
