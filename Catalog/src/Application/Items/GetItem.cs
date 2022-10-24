using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Variants;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items;

public record GetItem(string ItemId) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<GetItem, ItemDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemDto?> Handle(GetItem request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.Group)
                .FirstOrDefaultAsync(p => p.Id == request.ItemId);

            if (item == null) return null;

            return new ItemDto(item.Id, item.Name, item.Description,
                            item.Group is not null ? new Groups.ItemGroupDto(item.Group.Id, item.Group.Name, item.Group.Description, item.Group?.Parent?.Id) : null,
                            GetImageUrl(item.Image), item.Price.GetValueOrDefault(), item.CompareAtPrice, item.HasVariants, (ItemVisibility?)item.Visibility,
                            item.AttributeValues.Select(x => x.ToDto()));
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
