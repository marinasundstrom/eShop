using MediatR;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

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

            return variant.ToDto();
        }
    }
}
