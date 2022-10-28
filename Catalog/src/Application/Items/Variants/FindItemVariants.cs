using MediatR;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;
namespace YourBrand.Catalog.Application.Items.Variants;

public record FindItemVariants(string ItemId, Dictionary<string, string?> SelectedOptions) : IRequest<IEnumerable<ItemDto>>
{
    public class Handler : IRequestHandler<FindItemVariants, IEnumerable<ItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ItemsService _itemVariantsService;

        public Handler(IApplicationDbContext context, ItemsService itemVariantsService)
        {
            _context = context;
            _itemVariantsService = itemVariantsService;
        }

        public async Task<IEnumerable<ItemDto>> Handle(FindItemVariants request, CancellationToken cancellationToken)
        {
            var variants = await _itemVariantsService.FindVariantCore(request.ItemId, null, request.SelectedOptions);

            return variants
                .OrderBy(x => x.Id)
                .Select(item =>
                {
                    return item.ToDto();
                });
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
