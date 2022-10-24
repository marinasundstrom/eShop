using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Attributes.Groups;

public record UpdateItemAttributeGroup(string ItemId, string AttributeGroupId, ApiUpdateItemAttributeGroup Data) : IRequest<AttributeGroupDto>
{
    public class Handler : IRequestHandler<UpdateItemAttributeGroup, AttributeGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeGroupDto> Handle(UpdateItemAttributeGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .Include(x => x.AttributeGroups)
            .FirstAsync(x => x.Id == request.ItemId);

            var attributeGroup = item.AttributeGroups
                .First(x => x.Id == request.AttributeGroupId);

            attributeGroup.Name = request.Data.Name;
            attributeGroup.Description = request.Data.Description;

            await _context.SaveChangesAsync();

            return new AttributeGroupDto(attributeGroup.Id, attributeGroup.Name, attributeGroup.Description);
        }
    }
}
