using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Products.Attributes;

public record AddProductAttribute(string ProductId, string AttributeId, string ValueId) : IRequest<ProductAttributeDto>
{
    public class Handler : IRequestHandler<AddProductAttribute, ProductAttributeDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductAttributeDto> Handle(AddProductAttribute request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstAsync(attribute => attribute.Id == request.ProductId, cancellationToken);

            var attribute = await _context.Attributes
                .Include(x => x.Values)
                .FirstOrDefaultAsync(attribute => attribute.Id == request.AttributeId, cancellationToken);

            var value = attribute!.Values
                .First();

            Domain.Entities.ProductAttribute productAttribute = new()
            {
                ProductId = item.Id,
                AttributeId = attribute.Id,
                Value = value!
            };

            item.ProductAttributes.Add(productAttribute);

            await _context.SaveChangesAsync(cancellationToken);

            return productAttribute.ToDto();
        }
    }
}
