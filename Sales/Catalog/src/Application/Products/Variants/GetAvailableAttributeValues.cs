using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Products.Variants;

public record GetAvailableAttributeValues(string ProductId, string AttributeId, IDictionary<string, string?> SelectedAttributeValues) : IRequest<IEnumerable<AttributeValueDto>>
{
    public class Handler : IRequestHandler<GetAvailableAttributeValues, IEnumerable<AttributeValueDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AttributeValueDto>> Handle(GetAvailableAttributeValues request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> variants = await _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ParentProduct)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Attribute)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Value)
                .Where(pv => pv.ParentProduct!.Id == request.ProductId)
                .ToArrayAsync();

            foreach (var selectedAttribute in request.SelectedAttributeValues)
            {
                if (selectedAttribute.Value is null)
                    continue;

                variants = variants.Where(x => x.AttributeValues.Any(vv => vv.Attribute.Id == selectedAttribute.Key && vv.Value.Id == selectedAttribute.Value));
            }

            var values = variants
                .SelectMany(x => x.AttributeValues)
                .DistinctBy(x => x.Attribute)
                .Where(x => x.Attribute.Id == request.AttributeId)
                .Select(x => x.Value);

            return values.Select(x => new AttributeValueDto(x.Id, x.Name, x.Seq));
        }
    }
}
