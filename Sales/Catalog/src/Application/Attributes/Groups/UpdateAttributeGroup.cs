using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Attributes.Groups;

public record UpdateAttributeGroup(string Id, ApiUpdateProductAttributeGroup Data) : IRequest<AttributeGroupDto>
{
    public class Handler : IRequestHandler<UpdateAttributeGroup, AttributeGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeGroupDto> Handle(UpdateAttributeGroup request, CancellationToken cancellationToken)
        {
            var attributeGroup = await _context.AttributeGroups
                .FirstAsync(x => x.Id == request.Id, cancellationToken);

            attributeGroup.Name = request.Data.Name;
            attributeGroup.Description = request.Data.Description;

            await _context.SaveChangesAsync();

            return new AttributeGroupDto(attributeGroup.Id, attributeGroup.Name, attributeGroup.Description);
        }
    }
}
