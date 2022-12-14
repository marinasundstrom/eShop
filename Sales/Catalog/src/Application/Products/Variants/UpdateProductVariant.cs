using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;
namespace YourBrand.Catalog.Application.Products.Variants;

public record UpdateProductVariant(string ProductId, string ProductVariantId, ApiUpdateProductVariant Data) : IRequest<ProductDto>
{
    public class Handler : IRequestHandler<UpdateProductVariant, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        private ProductsService _itemVariantsService;

        public Handler(IApplicationDbContext context, ProductsService itemVariantsService)
        {
            _context = context;
            _itemVariantsService = itemVariantsService;
        }

        public async Task<ProductDto> Handle(UpdateProductVariant request, CancellationToken cancellationToken)
        {
            var match = (await _itemVariantsService.FindVariantCore(request.ProductId, request.ProductVariantId, request.Data.Attributes.ToDictionary(x => x.AttributeId, x => x.ValueId)!))
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
                    .ThenInclude(o => o.AttributeValues)
                    .ThenInclude(o => o.Attribute)
                .Include(pv => pv.Variants)
                    .ThenInclude(o => o.AttributeValues)
                    .ThenInclude(o => o.Value)
                .Include(pv => pv.Attributes)
                    .ThenInclude(o => o.Values)
                .FirstAsync(x => x.Id == request.ProductId);

            var variant = item.Variants.First(x => x.Id == request.ProductVariantId);

            variant.Name = request.Data.Name;
            variant.Description = request.Data.Description;
            variant.Price = request.Data.Price;
            variant.CompareAtPrice = request.Data.CompareAtPrice;

            foreach (var v in request.Data.Attributes)
            {
                if (v.Id == null)
                {
                    var option = item.Attributes.First(x => x.Id == v.AttributeId);

                    var value2 = option.Values.First(x => x.Id == v.ValueId);

                    variant.AttributeValues.Add(new ProductAttributeValue()
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

            return variant.ToDto();
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
