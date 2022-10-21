using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Application.Attributes;
using Catalog.Domain;
using Catalog.Domain.Entities;

namespace Catalog.Application.Products.Attributes;

public record CreateProductAttributeValue(string ProductId, string AttributeId, ApiCreateProductAttributeValue Data) : IRequest<AttributeValueDto>
{
    public class Handler : IRequestHandler<CreateProductAttributeValue, AttributeValueDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AttributeValueDto> Handle(CreateProductAttributeValue request, CancellationToken cancellationToken)
        {
            var product = await _context.Products
            .FirstAsync(x => x.Id == request.ProductId);

            var attribute = await _context.Attributes
                .FirstAsync(x => x.Id == request.AttributeId);

            var value = new AttributeValue
            {
                Name = request.Data.Name
            };

            attribute.Values.Add(value);

            await _context.SaveChangesAsync();

            return new AttributeValueDto(value.Id, value.Name, value.Seq);
        }
    }
}
