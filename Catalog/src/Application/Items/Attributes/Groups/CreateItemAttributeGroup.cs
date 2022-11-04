using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Attributes.Groups;

public record CreateItemAttributeGroup(string ItemId, ApiCreateItemAttributeGroup Data) : IRequest<AttributeGroupDto>
{
    public class Handler : IRequestHandler<CreateItemAttributeGroup, AttributeGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeGroupDto> Handle(CreateItemAttributeGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .FirstAsync(x => x.Id == request.ItemId);

            var group = new AttributeGroup(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name,
                Description = request.Data.Description
            };

            item.AttributeGroups.Add(group);

            await _context.SaveChangesAsync();

            return new AttributeGroupDto(group.Id, group.Name, group.Description);
        }
    }
}
