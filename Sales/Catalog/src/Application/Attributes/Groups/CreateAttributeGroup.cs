using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Attributes.Groups;

public record CreateAttributeGroup(ApiCreateProductAttributeGroup Data) : IRequest<AttributeGroupDto>
{
    public class Handler : IRequestHandler<CreateAttributeGroup, AttributeGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeGroupDto> Handle(CreateAttributeGroup request, CancellationToken cancellationToken)
        {
            var group = new AttributeGroup(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name,
                Description = request.Data.Description
            };

            _context.AttributeGroups.Add(group);

            await _context.SaveChangesAsync(cancellationToken);

            return new AttributeGroupDto(group.Id, group.Name, group.Description);
        }
    }
}
