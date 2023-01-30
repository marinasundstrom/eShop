using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Attributes.Values;

public record CreateProductAttributeValue(string Id, ApiCreateProductAttributeValue Data) : IRequest<AttributeValueDto>
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
            var attribute = await _context.Attributes
                .FirstAsync(x => x.Id == request.Id);

            var value = new AttributeValue(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name
            };

            attribute.Values.Add(value);

            await _context.SaveChangesAsync(cancellationToken);

            return new AttributeValueDto(value.Id, value.Name, value.Seq);
        }
    }
}