using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record CreateProduct(string Name, string Handle, string StoreId, bool HasVariants, string? Description, long? BrandId, long? GroupId, string? Sku, decimal? Price, ProductVisibility? Visibility) : IRequest<ProductDto?>
{
    public sealed class Handler : IRequestHandler<CreateProduct, ProductDto?>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(CreateProduct request, CancellationToken cancellationToken)
        {
            if(await _context.Products.AnyAsync(x => x.Name == request.Name)) 
            {
                throw new Exception("Product with name already exists");
            }

            if(await _context.Products.AnyAsync(x => x.Handle == request.Handle)) 
            {
                throw new Exception("Handle already in use");
            }

            var group = await _context.ProductGroups
                .Include(x => x.Attributes)
                .Include(x => x.Options)
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.Id == request.GroupId);

            if(group is null) 
            {
                throw new Exception("Group not found");
            }

            if(!group.AllowItems) 
            {
                throw new Exception("Cannot add items to category");
            }

            Brand? brand = null;

            if(request.BrandId is not null) 
            {
                brand = await _context.Brands
                    .FirstOrDefaultAsync(x => x.Id == request.BrandId);

                if(brand is null) 
                {
                    throw new Exception();
                }
            }

            var item = new Product(request.Name, request.Handle)
            {
                Name = request.Name,
                Description = request.Description,
                StoreId = request.StoreId,
                Brand = brand,
                Group = group,
                Price = request.Price,
                HasVariants = request.HasVariants
            };

            group!.IncrementProductCount();

            /*
            foreach (var attribute in group!.Attributes)
            {
                item.ProductAttributes.Add(attribute);
            }
            */

            foreach (var option in group!.Options)
            {
                item.Options.Add(option);
            }

            if (request.Visibility == null)
            {
                item.Visibility = Domain.Enums.ProductVisibility.Unlisted;
            }
            else
            {
                item.Visibility = request.Visibility == ProductVisibility.Listed ? Domain.Enums.ProductVisibility.Listed : Domain.Enums.ProductVisibility.Unlisted;
            }

            _context.Products.Add(item);

            await _context.SaveChangesAsync();

            return item.ToDto();
        }
    }
}