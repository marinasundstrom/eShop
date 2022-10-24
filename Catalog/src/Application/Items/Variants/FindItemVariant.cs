using MediatR;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Variants;

public record FindItemVariant(string ItemId, Dictionary<string, string?> SelectedOptions) : IRequest<ItemDto?>
{
    public class Handler : IRequestHandler<FindItemVariant, ItemDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly ItemsService _variantsService;

        public Handler(IApplicationDbContext context, ItemsService variantsService)
        {
            _context = context;
            _variantsService = variantsService;
        }

        public async Task<ItemDto?> Handle(FindItemVariant request, CancellationToken cancellationToken)
        {
            var variant = (await _variantsService.FindVariantCore(request.ItemId, null, request.SelectedOptions))
                .SingleOrDefault();

            if (variant is null) return null;

            return new ItemDto(variant.Id, variant.Name, variant.Description,
                variant.Group is not null ? new Groups.ItemGroupDto(variant.Group.Id, variant.Group.Name, variant.Group.Description, null) : null,
                GetImageUrl(variant.Image), variant.Price, variant.HasVariants, (ItemVisibility?)variant.Visibility,
                variant.AttributeValues.Select(x => x.ToDto()));
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
