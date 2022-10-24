using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Attributes;

public record CreateItemAttribute(string ItemId, ApiCreateItemAttribute Data) : IRequest<AttributeDto>
{
    public class Handler : IRequestHandler<CreateItemAttribute, AttributeDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeDto> Handle(CreateItemAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .FirstAsync(attribute => attribute.Id == request.ItemId);

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

            item.Attributes.Add(attribute);

            await _context.SaveChangesAsync();

            return attribute.ToDto();    
        }
    }
}
