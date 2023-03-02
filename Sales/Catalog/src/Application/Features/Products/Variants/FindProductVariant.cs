using MediatR;

namespace YourBrand.Catalog.Features.Products.Variants;

public record FindProductVariant(string ProductIdOrHandle, Dictionary<string, string?> SelectedOptions) : IRequest<ProductDto?>
{
    public class Handler : IRequestHandler<FindProductVariant, ProductDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly ProductsService _variantsService;

        public Handler(IApplicationDbContext context, ProductsService variantsService)
        {
            _context = context;
            _variantsService = variantsService;
        }

        public async Task<ProductDto?> Handle(FindProductVariant request, CancellationToken cancellationToken)
        {
            var variant = (await _variantsService.FindVariantCore(request.ProductIdOrHandle, null, request.SelectedOptions))
                .SingleOrDefault();

            if (variant is null) return null;

            return variant.ToDto();
        }
    }
}