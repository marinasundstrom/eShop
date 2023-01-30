using MediatR;

using Microsoft.EntityFrameworkCore;
namespace YourBrand.Catalog.Features.Products.Variants;

public record CreateProductVariant(string ProductId, ApiCreateProductVariant Data) : IRequest<ProductDto>
{
    public class Handler : IRequestHandler<CreateProductVariant, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ProductsService _itemVariantsService;

        public Handler(IApplicationDbContext context, ProductsService itemVariantsService)
        {
            _context = context;
            _itemVariantsService = itemVariantsService;
        }

        public async Task<ProductDto> Handle(CreateProductVariant request, CancellationToken cancellationToken)
        {
            Product? match = (await _itemVariantsService.FindVariantCore(request.ProductId, null, request.Data.Attributes.ToDictionary(x => x.AttributeId, x => x.ValueId)!))
                .SingleOrDefault();

            if (match is not null)
            {
                throw new VariantAlreadyExistsException("Variant with the same options already exists.");
            }

            var item = await _context.Products
                .AsSplitQuery()
                .Include(pv => pv.ParentProduct)
                    .ThenInclude(pv => pv!.Group)
                .Include(pv => pv.Variants)
                    .ThenInclude(o => o.ProductAttributes)
                    .ThenInclude(o => o.Attribute)
                .Include(pv => pv.Variants)
                    .ThenInclude(o => o.ProductAttributes)
                    .ThenInclude(o => o.Value)
                .FirstAsync(x => x.Id == request.ProductId);

            var variant = new Product(request.Data.Id ?? Guid.NewGuid().ToString(), request.Data.Name)
            {
                Description = request.Data.Description,
                Price = request.Data.Price
            };

            foreach (var value in request.Data.Attributes)
            {
                var attribute = _context.Attributes.First(x => x.Id == value.AttributeId);

                var value2 = attribute.Values.First(x => x.Id == value.ValueId);

                variant.ProductAttributes.Add(new ProductAttribute()
                {
                    Attribute = attribute,
                    Value = value2
                });
            }

            item.Variants.Add(variant);

            await _context.SaveChangesAsync();

            return variant.ToDto();
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}