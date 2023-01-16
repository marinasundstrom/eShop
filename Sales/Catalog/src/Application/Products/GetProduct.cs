using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Products.Groups;
using YourBrand.Catalog.Application.Products.Variants;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;

namespace YourBrand.Catalog.Application.Products;

public record GetProduct(string ProductId) : IRequest<ProductDto?>
{
    public class Handler : IRequestHandler<GetProduct, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(GetProduct request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ParentProduct)
                    .ThenInclude(pv => pv!.Group)
                .Include(pv => pv.Group)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Attribute)
                    .ThenInclude(o => o.Group)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Attribute)
                    .ThenInclude(pv => pv.Values)
                .Include(pv => pv.ProductAttributes)
                    .ThenInclude(pv => pv.Value)
                .Include(pv => pv.Options)
                    .ThenInclude(pv => pv.Group)
                .Include(pv => pv.Options)
                    .ThenInclude(pv => pv.Values)
                .Include(pv => pv.Options)
                    .ThenInclude(pv => pv.DefaultValue)
                .FirstOrDefaultAsync(p => p.Id == request.ProductId);

            return item?.ToDto();
        }
    }
}
