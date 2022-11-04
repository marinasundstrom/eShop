using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Attributes;

public record CreateItemAttributeValue(string ItemId, string AttributeId, ApiCreateItemAttributeValue Data) : IRequest<AttributeValueDto>
{
    public class Handler : IRequestHandler<CreateItemAttributeValue, AttributeValueDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeValueDto> Handle(CreateItemAttributeValue request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .FirstAsync(x => x.Id == request.ItemId);

            var attribute = await _context.Attributes
                .FirstAsync(x => x.Id == request.AttributeId);

            var value = new AttributeValue(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name
            };

            attribute.Values.Add(value);

            await _context.SaveChangesAsync();

            return new AttributeValueDto(value.Id, value.Name, value.Seq);
        }
    }
}
