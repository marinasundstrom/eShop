using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Items.Groups;
using YourBrand.Catalog.Application.Items.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

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
                .Include(x => x.Attributes)
                .Include(x => x.Options)
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

            foreach(var attribute in group!.Attributes) 
            {
                item.Attributes.Add(attribute);
            }

            foreach(var option in group.Options) 
            {
                item.Options.Add(option);
            }

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

            return item.ToDto();
        }
    }
}
