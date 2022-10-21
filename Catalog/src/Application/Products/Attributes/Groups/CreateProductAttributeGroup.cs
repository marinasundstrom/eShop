using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Application.Attributes;
using Catalog.Domain;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Attributes.Groups;

public record CreateProductAttributeGroup(string ProductId, ApiCreateProductAttributeGroup Data) : IRequest<AttributeGroupDto>
{
    public class Handler : IRequestHandler<CreateProductAttributeGroup, AttributeGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeGroupDto> Handle(CreateProductAttributeGroup request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            var group = new AttributeGroup()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Data.Name,
                Description = request.Data.Description
            };

            product.AttributeGroups.Add(group);

            await _context.SaveChangesAsync();

            return new AttributeGroupDto(group.Id, group.Name, group.Description);
        }
    }
}
