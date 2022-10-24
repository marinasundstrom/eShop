using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items;

public record CreateItem(string? Id, string Name, bool HasVariants, string? Description, string? GroupId, decimal? Price, ItemVisibility? Visibility) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<CreateItem, ItemDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ItemDto?> Handle(CreateItem request, CancellationToken cancellationToken)
        {
            var group = await _context.ItemGroups
            .FirstOrDefaultAsync(x => x.Id == request.GroupId);

            var item = new Item()
            {
                Id =  request.Id ?? Guid.NewGuid().ToString(),
                Name = request.Name,
                Description = request.Description,
                Group = group,
                Price = request.Price,
                HasVariants = request.HasVariants
            };

            if (request.Visibility == null)
            {
                item.Visibility = Domain.Enums.ItemVisibility.Unlisted;
            }
            else
            {
                item.Visibility = request.Visibility == ItemVisibility.Listed ? Domain.Enums.ItemVisibility.Listed : Domain.Enums.ItemVisibility.Unlisted;
            }

            _context.Items.Add(item);

            await _context.SaveChangesAsync();

            return new ItemDto(item.Id, item.Name, item.Description,
                item.Group is not null ? new Groups.ItemGroupDto(item.Group.Id, item.Group.Name, item.Group.Description, item.Group?.Parent?.Id) : null,
                GetImageUrl(item.Image), item.Price, item.HasVariants, (ItemVisibility?)item.Visibility,
                item.AttributeValues.Select(x => x.ToDto()));

        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
