using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Variants;

public record CreateItemVariant(string ItemId, ApiCreateItemVariant Data) : IRequest<ItemDto>
{
    public class Handler : IRequestHandler<CreateItemVariant, ItemDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ItemsService _itemVariantsService;

        public Handler(IApplicationDbContext context, ItemsService itemVariantsService)
        {
            _context = context;
            _itemVariantsService = itemVariantsService;
        }

        public async Task<ItemDto> Handle(CreateItemVariant request, CancellationToken cancellationToken)
        {
            Item? match = (await _itemVariantsService.FindVariantCore(request.ItemId, null, request.Data.Attributes.ToDictionary(x => x.OptionId, x => x.ValueId)!))
                .SingleOrDefault();

            if (match is not null)
            {
                throw new VariantAlreadyExistsException("Variant with the same options already exists.");
            }

            var item = await _context.Items
                .AsSplitQuery()
                .Include(pv => pv.Variants)
                    .ThenInclude(o => o.AttributeValues)
                    .ThenInclude(o => o.Attribute)
                .Include(pv => pv.Variants)
                    .ThenInclude(o => o.AttributeValues)
                    .ThenInclude(o => o.Value)
                .Include(pv => pv.Attributes)
                    .ThenInclude(o => o.Values)
                .FirstAsync(x => x.Id == request.ItemId);

            var variant = new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Data.Name,
                Description = request.Data.Description,
                Price = request.Data.Price
            };

            foreach (var value in request.Data.Attributes)
            {
                var option = item.Attributes.First(x => x.Id == value.OptionId);

                var value2 = option.Values.First(x => x.Id == value.ValueId);

                variant.AttributeValues.Add(new ItemAttributeValue()
                {
                    Attribute = option,
                    Value = value2
                });
            }

            item.Variants.Add(variant);

            await _context.SaveChangesAsync();

            return new ItemDto(variant.Id, variant.Name, variant.Description,
                            variant.Group is not null ? new Groups.ItemGroupDto(variant.Group.Id, variant.Group.Name, variant.Group.Description, variant.Group?.Parent?.Id) : null,
                            GetImageUrl(item.Image), variant.Price, variant.HasVariants, (ItemVisibility?)variant.Visibility,
                            variant.AttributeValues.Select(x => x.ToDto()));
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
