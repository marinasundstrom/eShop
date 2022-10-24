using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Variants;

public record UpdateItemVariant(string ItemId, string ItemVariantId, ApiUpdateItemVariant Data) : IRequest<ItemDto>
{
    public class Handler : IRequestHandler<UpdateItemVariant, ItemDto>
    {
        private readonly IApplicationDbContext _context;
        private ItemsService _itemVariantsService;

        public Handler(IApplicationDbContext context, ItemsService itemVariantsService)
        {
            _context = context;
            _itemVariantsService = itemVariantsService;
        }
        
        public async Task<ItemDto> Handle(UpdateItemVariant request, CancellationToken cancellationToken)
        {
            var match = (await _itemVariantsService.FindVariantCore(request.ItemId, request.ItemVariantId, request.Data.Attributes.ToDictionary(x => x.AttributeId, x => x.ValueId)!))
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

            var variant = item.Variants.First(x => x.Id == request.ItemId);

            variant.Name = request.Data.Name;
            variant.Description = request.Data.Description;
            variant.Price = request.Data.Price;

            foreach (var v in request.Data.Attributes)
            {
                if (v.Id == null)
                {
                    var option = item.Attributes.First(x => x.Id == v.AttributeId);

                    var value2 = option.Values.First(x => x.Id == v.ValueId);

                    variant.AttributeValues.Add(new ItemAttributeValue()
                    {
                        Attribute = option,
                        Value = value2
                    });
                }
                else
                {
                    var option = item.Attributes.First(x => x.Id == v.AttributeId);

                    var value2 = option.Values.First(x => x.Id == v.ValueId);

                    var value = variant.AttributeValues.First(x => x.Id == v.Id);

                    value.Attribute = option;
                    value.Value = value2;
                }
            }

            foreach (var v in variant.AttributeValues.ToList())
            {
                if (_context.Entry(v).State == EntityState.Added)
                    continue;

                var value = request.Data.Attributes.FirstOrDefault(x => x.Id == v.Id);

                if (value is null)
                {
                    variant.AttributeValues.Remove(v);
                }
            }

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
