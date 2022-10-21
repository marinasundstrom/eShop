using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Application.Attributes;
using Catalog.Domain;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Attributes;

public record CreateProductAttribute(string ProductId, ApiCreateProductAttribute Data) : IRequest<AttributeDto>
{
    public class Handler : IRequestHandler<CreateProductAttribute, AttributeDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeDto> Handle(CreateProductAttribute request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
            .FirstAsync(attribute => attribute.Id == request.ProductId);

            var group = await _context.AttributeGroups
                .FirstOrDefaultAsync(attribute => attribute.Id == request.Data.GroupId);

            Domain.Entities.Attribute attribute = new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Data.Name,
                Description = request.Data.Description,
                Group = group,
                ForVariant = request.Data.ForVariant,
                IsMainAttribute = request.Data.IsMainAttribute
            };

            foreach (var v in request.Data.Values)
            {
                var value = new AttributeValue
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = v.Name
                };

                attribute.Values.Add(value);
            }

            product.Attributes.Add(attribute);

            await _context.SaveChangesAsync();

            return attribute.ToDto();    
        }
    }
}
