using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Items.Variants;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Variants;

public record GetItemVariant(string ItemId, string ItemVariantId) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<GetItemVariant, ItemDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemDto?> Handle(GetItemVariant request, CancellationToken cancellationToken)
        {
            var itemVariant = await _context.Items
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Variants)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Attribute)
                //.ThenInclude(o => o.DefaultValue)
                .Include(pv => pv.AttributeValues)
                .ThenInclude(pv => pv.Value)
                .FirstOrDefaultAsync(pv => pv.ParentItem!.Id == request.ItemId && pv.Id == request.ItemVariantId);

            if(itemVariant is null) return null;

            return new ItemDto(itemVariant.Id, itemVariant.Name, itemVariant.Description,
                itemVariant is not null ? new Groups.ItemGroupDto(itemVariant.Id, itemVariant.Name, itemVariant.Description, null) : null,
                GetImageUrl(itemVariant!.Image), itemVariant.Price.GetValueOrDefault(), itemVariant.CompareAtPrice, itemVariant.HasVariants, (ItemVisibility?)itemVariant.Visibility,
                itemVariant.AttributeValues.Select(x => x.ToDto()));
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
