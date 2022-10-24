using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Variants;

public record GetItemVariantAttributes(string ItemId, string ItemVariantId) : IRequest<IEnumerable<ItemVariantAttributeDto>>
{
    public class Handler : IRequestHandler<GetItemVariantAttributes, IEnumerable<ItemVariantAttributeDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemVariantAttributeDto>> Handle(GetItemVariantAttributes request, CancellationToken cancellationToken)
        {
            var variantOptionValues = await _context.ItemAttributeValues
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Value)
                .Include(pv => pv.Attribute)
                //.ThenInclude(o => o.DefaultValue)
                .Include(pv => pv.Variant)
                .ThenInclude(p => p.ParentItem)
                .Where(pv => pv.Variant.ParentItem!.Id == request.ItemId && pv.Variant.Id == request.ItemVariantId)
                .ToArrayAsync();

            return variantOptionValues.Select(x => x.ToDto());
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}