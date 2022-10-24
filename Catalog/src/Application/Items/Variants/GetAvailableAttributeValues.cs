using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Variants;

public record GetAvailableAttributeValues(string ItemId, string AttributeId, IDictionary<string, string?> SelectedAttributeValues) : IRequest<IEnumerable<AttributeValueDto>>
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
            IEnumerable<Item> variants = await _context.Items
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ParentItem)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Attribute)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Value)
                .Where(pv => pv.ParentItem!.Id == request.ItemId)
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
