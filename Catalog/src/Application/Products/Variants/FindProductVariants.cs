using MediatR;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;
namespace YourBrand.Catalog.Application.Products.Variants;

public record FindProductVariants(string ProductId, Dictionary<string, string?> SelectedOptions) : IRequest<IEnumerable<ProductDto>>
{
    public class Handler : IRequestHandler<FindProductVariants, IEnumerable<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ProductsService _itemVariantsService;

        public Handler(IApplicationDbContext context, ProductsService itemVariantsService)
        {
            _context = context;
            _itemVariantsService = itemVariantsService;
        }

        public async Task<IEnumerable<ProductDto>> Handle(FindProductVariants request, CancellationToken cancellationToken)
        {
            var variants = await _itemVariantsService.FindVariantCore(request.ProductId, null, request.SelectedOptions);

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
